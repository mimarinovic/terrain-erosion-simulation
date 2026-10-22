using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(ErosionController))]
public class ErosionOverTimeController : MonoBehaviour
{
    public float erosionStepsPerSecond = 1f;
    public bool isEroding = false;

    public int totalRuns = 10;
    private int completedRuns = 0;

    private ErosionController erosionController;
    private Coroutine erosionCoroutine;

    private System.Action onErosionFinished;

    void Awake()
    {
        erosionController = GetComponent<ErosionController>();
    }

    public void StartErosion(System.Action onFinished = null)
    {
        if (!isEroding)
        {
            isEroding = true;
            completedRuns = 0;
            onErosionFinished = onFinished;
            erosionCoroutine = StartCoroutine(ErodeOverTime());
        }
    }

    public void StopErosion()
    {
        if (isEroding)
        {
            isEroding = false;
            if (erosionCoroutine != null)
                StopCoroutine(erosionCoroutine);
        }
    }

    private System.Collections.IEnumerator ErodeOverTime()
    {
        while (isEroding && completedRuns < totalRuns)
        {
            erosionController.Erode();
            completedRuns++;
            yield return new WaitForSeconds(1f / Mathf.Max(erosionStepsPerSecond, 0.01f));
        }
        isEroding = false;
        if (onErosionFinished != null)
        {
            onErosionFinished.Invoke();
            onErosionFinished = null;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ErosionOverTimeController))]
public class ErosionOverTimeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ErosionOverTimeController controller = (ErosionOverTimeController)target;
        if (!controller.isEroding)
        {
            if (GUILayout.Button("Start Erosion Over Time"))
            {
                controller.StartErosion();
            }
        }
        else
        {
            if (GUILayout.Button("Stop Erosion"))
            {
                controller.StopErosion();
            }
        }
    }
}
#endif
