using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErosionUIController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField enterErosionIterations;
    public TMP_InputField enterInertia;
    public TMP_InputField enterSedimentCapacityFactor;
    public TMP_InputField enterMinSedimentCapacity;
    public TMP_InputField enterDepositSpeed;
    public TMP_InputField enterErodeSpeed;
    public TMP_InputField enterEvaporateSpeed;
    public TMP_InputField enterGravity;
    public TMP_InputField enterStartSpeed;
    public TMP_InputField enterStartWater;
    public TMP_InputField enterMaxLifetime;
    public TMP_InputField enterErosionStepsPerSecond;
    public TMP_InputField enterTotalRuns;
    public GameObject settingsContent;
    public Button seeErosionSettingsButton;

    [Header("Simulation Reference")]
    public ErosionController erosionController;

    public Slider inertiaSlider;
    public Slider sedimentCapacitySlider;
    public Slider minSedimentCapacitySlider;
    public Slider erodeSpeedSlider;
    public Slider evaporateSpeedSlider;
    public Slider gravitySlider;
    public Slider startSpeedSlider;
    public Slider startWaterSlider;
    public Slider maxLifetimeSlider;

    public Button runErosionButton;
    public Button runErosionOverTimeButton;
    public ErosionOverTimeController erosionOverTimeController;

    private bool settingsVisible = false;
    private bool isErosionOverTimeRunning = false;

    void Start()
    {
        if (enterErosionIterations != null) enterErosionIterations.text = "10000";
        if (enterInertia != null) enterInertia.text = "0,99";
        if (enterSedimentCapacityFactor != null) enterSedimentCapacityFactor.text = "0,6";
        if (enterMinSedimentCapacity != null) enterMinSedimentCapacity.text = "0,005";
        if (enterDepositSpeed != null) enterDepositSpeed.text = "1";
        if (enterErodeSpeed != null) enterErodeSpeed.text = "0,1";
        if (enterEvaporateSpeed != null) enterEvaporateSpeed.text = "0,01";
        if (enterGravity != null) enterGravity.text = "1";
        if (enterStartSpeed != null) enterStartSpeed.text = "1";
        if (enterStartWater != null) enterStartWater.text = "2";
        if (enterMaxLifetime != null) enterMaxLifetime.text = "30";
        if (enterErosionStepsPerSecond != null) enterErosionStepsPerSecond.text = "10";
        if (enterTotalRuns != null) enterTotalRuns.text = "100";

        if (inertiaSlider != null) {
            inertiaSlider.minValue = 0f;
            inertiaSlider.maxValue = 1f;
            inertiaSlider.value = 0.99f;
            inertiaSlider.onValueChanged.AddListener(OnInertiaSliderChanged);
        }
        if (enterInertia != null) enterInertia.onEndEdit.AddListener(OnInertiaInputChanged);

        if (sedimentCapacitySlider != null) {
            sedimentCapacitySlider.minValue = 0f;
            sedimentCapacitySlider.maxValue = 2f;
            sedimentCapacitySlider.value = 0.6f;
            sedimentCapacitySlider.onValueChanged.AddListener(v => { if (enterSedimentCapacityFactor != null) enterSedimentCapacityFactor.text = v.ToString("0.###"); });
        }
        if (enterSedimentCapacityFactor != null) enterSedimentCapacityFactor.onEndEdit.AddListener(v => { if (sedimentCapacitySlider != null && float.TryParse(v, out float val)) sedimentCapacitySlider.value = val; });

        if (minSedimentCapacitySlider != null) {
            minSedimentCapacitySlider.minValue = 0.001f;
            minSedimentCapacitySlider.maxValue = 0.05f;
            minSedimentCapacitySlider.value = 0.005f;
            minSedimentCapacitySlider.onValueChanged.AddListener(v => { if (enterMinSedimentCapacity != null) enterMinSedimentCapacity.text = v.ToString("0.###"); });
        }
        if (enterMinSedimentCapacity != null) enterMinSedimentCapacity.onEndEdit.AddListener(v => { if (minSedimentCapacitySlider != null && float.TryParse(v, out float val)) minSedimentCapacitySlider.value = val; });

        if (erodeSpeedSlider != null) {
            erodeSpeedSlider.minValue = 0.01f;
            erodeSpeedSlider.maxValue = 0.5f;
            erodeSpeedSlider.value = 0.1f;
            erodeSpeedSlider.onValueChanged.AddListener(v => { if (enterErodeSpeed != null) enterErodeSpeed.text = v.ToString("0.###"); });
        }
        if (enterErodeSpeed != null) enterErodeSpeed.onEndEdit.AddListener(v => { if (erodeSpeedSlider != null && float.TryParse(v, out float val)) erodeSpeedSlider.value = val; });

        if (evaporateSpeedSlider != null) {
            evaporateSpeedSlider.minValue = 0f;
            evaporateSpeedSlider.maxValue = 1f;
            evaporateSpeedSlider.value = 0.01f;
            evaporateSpeedSlider.onValueChanged.AddListener(v => { if (enterEvaporateSpeed != null) enterEvaporateSpeed.text = v.ToString("0.###"); });
        }
        if (enterEvaporateSpeed != null) enterEvaporateSpeed.onEndEdit.AddListener(v => { if (evaporateSpeedSlider != null && float.TryParse(v, out float val)) evaporateSpeedSlider.value = val; });

        if (gravitySlider != null) {
            gravitySlider.minValue = 0f;
            gravitySlider.maxValue = 100f;
            gravitySlider.value = 1f;
            gravitySlider.onValueChanged.AddListener(v => { if (enterGravity != null) enterGravity.text = v.ToString("0.###"); });
        }
        if (enterGravity != null) enterGravity.onEndEdit.AddListener(v => { if (gravitySlider != null && float.TryParse(v, out float val)) gravitySlider.value = val; });

        if (startSpeedSlider != null) {
            startSpeedSlider.minValue = 0f;
            startSpeedSlider.maxValue = 2f;
            startSpeedSlider.value = 1f;
            startSpeedSlider.onValueChanged.AddListener(v => { if (enterStartSpeed != null) enterStartSpeed.text = v.ToString("0.###"); });
        }
        if (enterStartSpeed != null) enterStartSpeed.onEndEdit.AddListener(v => { if (startSpeedSlider != null && float.TryParse(v, out float val)) startSpeedSlider.value = val; });

        if (startWaterSlider != null) {
            startWaterSlider.minValue = 0f;
            startWaterSlider.maxValue = 5f;
            startWaterSlider.value = 2f;
            startWaterSlider.onValueChanged.AddListener(v => { if (enterStartWater != null) enterStartWater.text = v.ToString("0.###"); });
        }
        if (enterStartWater != null) enterStartWater.onEndEdit.AddListener(v => { if (startWaterSlider != null && float.TryParse(v, out float val)) startWaterSlider.value = val; });

        if (maxLifetimeSlider != null) {
            maxLifetimeSlider.minValue = 1;
            maxLifetimeSlider.maxValue = 100;
            maxLifetimeSlider.value = 30;
            maxLifetimeSlider.onValueChanged.AddListener(v => { if (enterMaxLifetime != null) enterMaxLifetime.text = ((int)v).ToString(); });
        }
        if (enterMaxLifetime != null) enterMaxLifetime.onEndEdit.AddListener(v => { if (maxLifetimeSlider != null && int.TryParse(v, out int val)) maxLifetimeSlider.value = val; });

        if (settingsContent != null) settingsContent.SetActive(false);
        settingsVisible = false;
        if (seeErosionSettingsButton != null)
            seeErosionSettingsButton.onClick.AddListener(ToggleSettingsPanel);
        if (runErosionButton != null)
            runErosionButton.onClick.AddListener(OnRunErosionClicked);
        if (runErosionOverTimeButton != null)
            runErosionOverTimeButton.onClick.AddListener(OnRunErosionOverTimeClicked);
    }

    void ToggleSettingsPanel()
    {
        settingsVisible = !settingsVisible;
        if (settingsContent != null) settingsContent.SetActive(settingsVisible);
    }

    void OnInertiaSliderChanged(float value)
    {
        if (enterInertia != null)
            enterInertia.text = value.ToString("0.###");
        if (erosionController != null)
            erosionController.inertia = value;
    }

    void OnInertiaInputChanged(string value)
    {
        if (float.TryParse(value, out float f))
        {
            f = Mathf.Clamp01(f);
            if (inertiaSlider != null)
                inertiaSlider.value = f;
            if (erosionController != null)
                erosionController.inertia = f;
        }
    }

    void OnRunErosionClicked()
    {
        ApplyErosionSettingsFromUI();
        if (erosionController == null) {
            return;
        }
        erosionController.Erode();
    }

    void OnRunErosionOverTimeClicked()
    {
        ApplyErosionSettingsFromUI();
        ApplyErosionOverTimeSettingsFromUI();
        if (!isErosionOverTimeRunning)
        {
            if (erosionOverTimeController != null)
            {
                erosionOverTimeController.StartErosion(OnErosionOverTimeFinished);
            }
            isErosionOverTimeRunning = true;
            if (runErosionOverTimeButton != null)
                runErosionOverTimeButton.GetComponentInChildren<TMP_Text>().text = "Stop erosion";
        }
        else
        {
            if (erosionOverTimeController != null)
                erosionOverTimeController.StopErosion();
            isErosionOverTimeRunning = false;
            if (runErosionOverTimeButton != null)
                runErosionOverTimeButton.GetComponentInChildren<TMP_Text>().text = "Run erosion over time";
        }
    }

    private void OnErosionOverTimeFinished()
    {
        isErosionOverTimeRunning = false;
        if (runErosionOverTimeButton != null)
            runErosionOverTimeButton.GetComponentInChildren<TMP_Text>().text = "Run erosion over time";
    }

    public void ApplyErosionSettingsFromUI()
    {
        if (erosionController == null) return;
        if (enterErosionIterations != null && int.TryParse(enterErosionIterations.text, out int iterations))
            erosionController.numErosionIterations = Mathf.Max(1, iterations);
        if (enterInertia != null && float.TryParse(enterInertia.text, out float inertia))
            erosionController.inertia = Mathf.Clamp01(inertia);
        if (enterSedimentCapacityFactor != null && float.TryParse(enterSedimentCapacityFactor.text, out float scf))
            erosionController.sedimentCapacityFactor = Mathf.Clamp(scf, 0f, 2f);
        if (enterMinSedimentCapacity != null && float.TryParse(enterMinSedimentCapacity.text, out float minSC))
            erosionController.minSedimentCapacity = Mathf.Clamp(minSC, 0.001f, 0.05f);
        if (enterDepositSpeed != null && float.TryParse(enterDepositSpeed.text, out float depositSpeed))
            erosionController.depositSpeed = depositSpeed;
        if (enterErodeSpeed != null && float.TryParse(enterErodeSpeed.text, out float erodeSpeed))
            erosionController.erodeSpeed = Mathf.Clamp(erodeSpeed, 0.01f, 0.5f);
        if (enterEvaporateSpeed != null && float.TryParse(enterEvaporateSpeed.text, out float evapSpeed))
            erosionController.evaporateSpeed = Mathf.Clamp01(evapSpeed);
        if (enterGravity != null && float.TryParse(enterGravity.text, out float gravity))
            erosionController.gravity = Mathf.Clamp(gravity, 0f, 100f);
        if (enterStartSpeed != null && float.TryParse(enterStartSpeed.text, out float startSpeed))
            erosionController.startSpeed = Mathf.Clamp(startSpeed, 0f, 2f);
        if (enterStartWater != null && float.TryParse(enterStartWater.text, out float startWater))
            erosionController.startWater = Mathf.Clamp(startWater, 0f, 10f);
        if (enterMaxLifetime != null && int.TryParse(enterMaxLifetime.text, out int maxLifetime))
            erosionController.maxLifetime = Mathf.Clamp(maxLifetime, 1, 100);
    }

    public void ApplyErosionOverTimeSettingsFromUI()
    {
        if (erosionOverTimeController == null) return;
        if (enterErosionStepsPerSecond != null && float.TryParse(enterErosionStepsPerSecond.text, out float steps))
            erosionOverTimeController.erosionStepsPerSecond = steps;
        if (enterTotalRuns != null && int.TryParse(enterTotalRuns.text, out int runs))
            erosionOverTimeController.totalRuns = runs;
    }
}
