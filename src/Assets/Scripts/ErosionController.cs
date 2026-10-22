using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TerrainMeshGenerator), typeof(HeightMapGenerator))]
public class ErosionController : MonoBehaviour
{
    public int numErosionIterations = 50000;
    public float inertia = 0.05f;
    public float sedimentCapacityFactor = 4;
    public float minSedimentCapacity = 0.01f;
    public float depositSpeed = 0.3f;
    public float erodeSpeed = 0.3f;
    public float evaporateSpeed = 0.01f;
    public float gravity = 4;
    public float startSpeed = 1;
    public float startWater = 1;
    public int erosionRadius = 3;
    public int maxLifetime = 30;

    private float[] lastHeightMap;
    private float[] currentHeightMap;

    public void Erode() {
        var meshGen = GetComponent<TerrainMeshGenerator>();
        int mapSize = meshGen.mapSize;
        MeshFilter mf = meshGen.transform.Find("Mesh Holder")?.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null) {
            Vector3[] verts = mf.sharedMesh.vertices;
            currentHeightMap = new float[mapSize * mapSize];
            for (int i = 0; i < verts.Length; i++)
                currentHeightMap[i] = verts[i].y / meshGen.meshHeight;
        } else {
            Debug.LogWarning("No mesh found to erode. Please generate a mesh first.");
            return;
        }
        lastHeightMap = (float[])currentHeightMap.Clone();
        HydraulicErosion(currentHeightMap, mapSize, numErosionIterations);
        meshGen.GenerateMeshFromHeightMap(currentHeightMap);
    }

    // CPU hydraulic erosion
    void HydraulicErosion(float[] map, int mapSize, int iterations) {
        System.Random prng = new System.Random();
        int[] dx = {-1, 1, 0, 0};
        int[] dy = {0, 0, -1, 1};
        for (int iter = 0; iter < iterations; iter++) {
            // randomly place a droplet
            float posX = prng.Next(erosionRadius, mapSize - erosionRadius);
            float posY = prng.Next(erosionRadius, mapSize - erosionRadius);
            float dirX = 0, dirY = 0;
            float speed = startSpeed;
            float water = startWater;
            float sediment = 0;
            for (int lifetime = 0; lifetime < maxLifetime; lifetime++) {
                int nodeX = (int)posX;
                int nodeY = (int)posY;
                int idx = nodeY * mapSize + nodeX;
                float gradX = 0, gradY = 0;
                float h = map[idx];
                for (int d = 0; d < 4; d++) {
                    int nx = nodeX + dx[d];
                    int ny = nodeY + dy[d];
                    if (nx >= 0 && nx < mapSize && ny >= 0 && ny < mapSize) {
                        float nh = map[ny * mapSize + nx];
                        gradX += (nh - h) * dx[d];
                        gradY += (nh - h) * dy[d];
                    }
                }
                float gradMag = Mathf.Sqrt(gradX * gradX + gradY * gradY) + 1e-6f;
                gradX /= gradMag;
                gradY /= gradMag;
                dirX = dirX * inertia - gradX * (1 - inertia);
                dirY = dirY * inertia - gradY * (1 - inertia);
                float len = Mathf.Sqrt(dirX * dirX + dirY * dirY);
                if (len == 0) break;
                dirX /= len;
                dirY /= len;
                posX += dirX;
                posY += dirY;
                if (posX < erosionRadius || posX >= mapSize - erosionRadius || posY < erosionRadius || posY >= mapSize - erosionRadius)
                    break;
                int newIdx = ((int)posY) * mapSize + (int)posX;
                float newH = map[newIdx];
                float deltaH = newH - h;
                float capacity = Mathf.Max(-deltaH * speed * water * sedimentCapacityFactor, minSedimentCapacity);
                if (sediment > capacity) {
                    float deposit = (sediment - capacity) * depositSpeed;
                    map[idx] += deposit;
                    sediment -= deposit;
                } else {
                    float erodeAmount = Mathf.Min((capacity - sediment) * erodeSpeed, map[idx]);
                    map[idx] -= erodeAmount;
                    sediment += erodeAmount;
                }
                speed = Mathf.Sqrt(Mathf.Max(0, speed * speed + deltaH * gravity));
                water *= (1 - evaporateSpeed);
                if (water < 0.01f) break;
            }
        }
    }

    public void SetCurrentHeightMap(float[] heightMap)
    {
        currentHeightMap = heightMap;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ErosionController))]
public class ErosionControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ErosionController ctrl = (ErosionController)target;
        if (GUILayout.Button("Run Erosion"))
        {
            ctrl.Erode();
        }
    }
}
#endif
