using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldGenUIController : MonoBehaviour
{
    [Header("UI References")]
    public Button seeSettingsButton;
    public GameObject worldGenPanel;
    public GameObject settingsContent; 
    public TMP_InputField enterSeed;
    public Toggle randomSeed;
    public Button generateMeshButton;
    public TMP_InputField enterOctaves;
    public TMP_InputField enterPersistence;
    public TMP_InputField enterLacunarity;
    public TMP_InputField enterScale;
    public TMP_InputField enterMapSize;
    public TMP_InputField enterMapHeight;

    [Header("Simulation References")]
    public HeightMapGenerator heightMapGenerator;
    public TerrainMeshGenerator terrainMeshGenerator;

    private bool settingsVisible = false;

    void Start()
    {
        randomSeed.isOn = true;
        if (enterOctaves != null) enterOctaves.text = "5";
        if (enterPersistence != null) enterPersistence.text = "0,6";
        if (enterLacunarity != null) enterLacunarity.text = "1,8";
        if (enterScale != null) enterScale.text = "2";
        if (enterMapSize != null) enterMapSize.text = "256";
        if (enterMapHeight != null) enterMapHeight.text = "60";

        worldGenPanel.SetActive(false);
        settingsVisible = false;
        seeSettingsButton.onClick.AddListener(ToggleSettingsPanel);
        generateMeshButton.onClick.AddListener(OnGenerateMeshClicked);
        randomSeed.onValueChanged.AddListener(OnRandomSeedToggled);
        enterSeed.onEndEdit.AddListener(OnSeedChanged);
        if (enterOctaves != null) enterOctaves.onEndEdit.AddListener(OnOctavesChanged);
        if (enterPersistence != null) enterPersistence.onEndEdit.AddListener(OnPersistenceChanged);
        if (enterLacunarity != null) enterLacunarity.onEndEdit.AddListener(OnLacunarityChanged);
        if (enterScale != null) enterScale.onEndEdit.AddListener(OnScaleChanged);
        if (enterMapSize != null) enterMapSize.onEndEdit.AddListener(OnMapSizeChanged);
        if (enterMapHeight != null) enterMapHeight.onEndEdit.AddListener(OnMapHeightChanged);
        UpdateSeedInteractable();

        OnGenerateMeshClicked();
    }

    void ToggleSettingsPanel()
    {
        settingsVisible = !settingsVisible;
        worldGenPanel.SetActive(settingsVisible);
    }

    void OnGenerateMeshClicked()
    {
        if (heightMapGenerator == null || terrainMeshGenerator == null) return;
        heightMapGenerator.randomizeSeed = randomSeed.isOn;
        if (!randomSeed.isOn)
        {
            int seed;
            if (int.TryParse(enterSeed.text, out seed))
                heightMapGenerator.seed = seed;
        }
        if (enterOctaves != null && int.TryParse(enterOctaves.text, out int octaves))
            heightMapGenerator.numOctaves = octaves;
        if (enterPersistence != null && float.TryParse(enterPersistence.text, out float persistence))
            heightMapGenerator.persistence = persistence;
        if (enterLacunarity != null && float.TryParse(enterLacunarity.text, out float lacunarity))
            heightMapGenerator.lacunarity = lacunarity;
        if (enterScale != null && float.TryParse(enterScale.text, out float scale))
            heightMapGenerator.initialScale = scale;
        if (enterMapSize != null && int.TryParse(enterMapSize.text, out int mapSize))
            terrainMeshGenerator.mapSize = mapSize;
        if (enterMapHeight != null && float.TryParse(enterMapHeight.text, out float meshHeight))
            terrainMeshGenerator.meshHeight = meshHeight;
        terrainMeshGenerator.GenerateMesh();
    }

    void OnRandomSeedToggled(bool isOn)
    {
        UpdateSeedInteractable();
    }

    void OnSeedChanged(string value)
    {
        if (heightMapGenerator != null && !randomSeed.isOn)
        {
            int seed;
            if (int.TryParse(value, out seed))
                heightMapGenerator.seed = seed;
        }
    }

    void UpdateSeedInteractable()
    {
        enterSeed.interactable = !randomSeed.isOn;
    }

    void OnOctavesChanged(string value)
    {
        if (heightMapGenerator != null && int.TryParse(value, out int octaves))
            heightMapGenerator.numOctaves = octaves;
    }
    void OnPersistenceChanged(string value)
    {
        if (heightMapGenerator != null && float.TryParse(value, out float persistence))
            heightMapGenerator.persistence = persistence;
    }
    void OnLacunarityChanged(string value)
    {
        if (heightMapGenerator != null && float.TryParse(value, out float lacunarity))
            heightMapGenerator.lacunarity = lacunarity;
    }
    void OnScaleChanged(string value)
    {
        if (heightMapGenerator != null && float.TryParse(value, out float scale))
            heightMapGenerator.initialScale = scale;
    }
    void OnMapSizeChanged(string value)
    {
        if (terrainMeshGenerator != null && int.TryParse(value, out int mapSize))
            terrainMeshGenerator.mapSize = mapSize;
    }
    void OnMapHeightChanged(string value)
    {
        if (terrainMeshGenerator != null && float.TryParse(value, out float meshHeight))
            terrainMeshGenerator.meshHeight = meshHeight;
    }
}
