using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(HeightMapGenerator))]
public class TerrainMeshGenerator : MonoBehaviour
{
    public int mapSize = 128;
    public float meshHeight = 20f;
    public Material terrainMaterial;
    private GameObject meshHolder;

    public void GenerateMesh()
    {
        if (meshHolder != null)
        {
            DestroyImmediate(meshHolder);
        }
        meshHolder = new GameObject("Mesh Holder");
        meshHolder.transform.parent = transform;
        meshHolder.transform.localPosition = Vector3.zero;
        meshHolder.transform.localRotation = Quaternion.identity;

        MeshFilter mf = meshHolder.AddComponent<MeshFilter>();
        MeshRenderer mr = meshHolder.AddComponent<MeshRenderer>();
        mr.sharedMaterial = terrainMaterial;

        var heightMapGen = GetComponent<HeightMapGenerator>();
        float[] heightMap = heightMapGen.GenerateHeightMap(mapSize);

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        Vector3[] vertices = new Vector3[mapSize * mapSize];
        Vector2[] uvs = new Vector2[mapSize * mapSize];
        int[] triangles = new int[(mapSize - 1) * (mapSize - 1) * 6];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int i = y * mapSize + x;
                float height = heightMap[i] * meshHeight;
                vertices[i] = new Vector3(x, height, y);
                uvs[i] = new Vector2(x / (float)(mapSize - 1), y / (float)(mapSize - 1));
            }
        }
        int t = 0;
        for (int y = 0; y < mapSize - 1; y++)
        {
            for (int x = 0; x < mapSize - 1; x++)
            {
                int i = y * mapSize + x;
                triangles[t++] = i;
                triangles[t++] = i + mapSize;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + mapSize;
                triangles[t++] = i + mapSize + 1;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mf.sharedMesh = mesh;
    }

    public void GenerateMeshFromHeightMap(float[] heightMap)
    {
        if (meshHolder != null)
        {
            DestroyImmediate(meshHolder);
        }
        meshHolder = new GameObject("Mesh Holder");
        meshHolder.transform.parent = transform;
        meshHolder.transform.localPosition = Vector3.zero;
        meshHolder.transform.localRotation = Quaternion.identity;

        MeshFilter mf = meshHolder.AddComponent<MeshFilter>();
        MeshRenderer mr = meshHolder.AddComponent<MeshRenderer>();
        mr.sharedMaterial = terrainMaterial;

        Vector3[] vertices = new Vector3[mapSize * mapSize];
        Vector2[] uvs = new Vector2[mapSize * mapSize];
        int[] triangles = new int[(mapSize - 1) * (mapSize - 1) * 6];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int i = y * mapSize + x;
                float height = heightMap[i] * meshHeight;
                vertices[i] = new Vector3(x, height, y);
                uvs[i] = new Vector2(x / (float)(mapSize - 1), y / (float)(mapSize - 1));
            }
        }
        int t = 0;
        for (int y = 0; y < mapSize - 1; y++)
        {
            for (int x = 0; x < mapSize - 1; x++)
            {
                int i = y * mapSize + x;
                triangles[t++] = i;
                triangles[t++] = i + mapSize;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + mapSize;
                triangles[t++] = i + mapSize + 1;
            }
        }
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mf.sharedMesh = mesh;

        var erosion = GetComponent<ErosionController>();
        if (erosion != null)
        {
            erosion.SetCurrentHeightMap((float[])heightMap.Clone());
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainMeshGenerator))]
public class TerrainMeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TerrainMeshGenerator gen = (TerrainMeshGenerator)target;
        if (GUILayout.Button("Generate Mesh"))
        {
            gen.GenerateMesh();
        }
    }
}
#endif
