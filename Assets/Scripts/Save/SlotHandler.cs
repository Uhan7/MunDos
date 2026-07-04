using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SlotHandler : MonoBehaviour
{
    [Header("Main References")]
    [SerializeField] TimelineManager timelineManager;
    [SerializeField] ThumbnailSlot thumbnailManager;
    [SerializeField] GameObject protag;
    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    //[SerializeField] GameObject slot3;
    //[SerializeField] GameObject slot4;
    //[SerializeField] GameObject slot5;
    //[SerializeField] GameObject slot6;
    // Sprite references
    [Header("Sprite References")]
    [SerializeField] private Sprite pastRibbon;
    [SerializeField] private Sprite presentRibbon;

    [HideInInspector] public List<SaveSlot> allSaveSlots = new List<SaveSlot>();
    [HideInInspector] public List<Sprite> ribbonSprites = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allSaveSlots.Add(slot1.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot2.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot3.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot4.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot5.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot6.GetComponent<SaveSlot>());

        ribbonSprites.Add(pastRibbon);
        ribbonSprites.Add(presentRibbon);

        // Reload from playerprefs
        allSaveSlots[0].slotIsFull = (PlayerPrefs.GetInt("SAVE_SLOT_0_ISFULL", 0) != 0);
        allSaveSlots[0].timelineSaved = PlayerPrefs.GetInt("SAVE_SLOT_0_TIMELINE", 1);
        allSaveSlots[1].slotIsFull = (PlayerPrefs.GetInt("SAVE_SLOT_1_ISFULL", 0) != 0);
        allSaveSlots[1].timelineSaved = PlayerPrefs.GetInt("SAVE_SLOT_1_TIMELINE", 1);
    }

    private void OnEnable()
    {
        StartCoroutine(SettingRibbons());
    }

    public void SelectSlot(int slotNum)
    {
        SettingsInfo.selectedSaveSlot = slotNum;
        for (int i = 0; i < allSaveSlots.Count; i++)
        {
            if (i != slotNum)
            {
                allSaveSlots[i].UnselectSlot();
            } else
            {
                allSaveSlots[i].SelectSlot();
            }
        }
    }

    public void Save()
    {
        allSaveSlots[SettingsInfo.selectedSaveSlot].ribbon.SetActive(true);
        allSaveSlots[SettingsInfo.selectedSaveSlot].slotIsFull = true;

        // Set ribbon color
        int currentTimeline = timelineManager.GetCurrentTimeline();
        allSaveSlots[SettingsInfo.selectedSaveSlot].timelineSaved = currentTimeline;
        allSaveSlots[SettingsInfo.selectedSaveSlot].SetRibbon(ribbonSprites[currentTimeline]);

        // Set thumbnail
        int area = DetermineArea();
        allSaveSlots[SettingsInfo.selectedSaveSlot].SetScreenshot(thumbnailManager.GetScreenshot(currentTimeline, area));

        SaveDetails();
        SaveSlotMetadata();
    }

    // Saves slot information to player prefs
    public void SaveDetails()
    {
        SettingsInfo.saveSlots[0, 0] = allSaveSlots[0].slotIsFull ? 1 : 0;
        SettingsInfo.saveSlots[0, 1] = allSaveSlots[0].timelineSaved;
        SettingsInfo.saveSlots[1, 0] = allSaveSlots[1].slotIsFull ? 1 : 0;
        SettingsInfo.saveSlots[1, 1] = allSaveSlots[1].timelineSaved;

        // PlayerPrefs for both slots
        PlayerPrefs.SetInt("SAVE_SLOT_0_ISFULL", SettingsInfo.saveSlots[0, 0]);
        PlayerPrefs.SetInt("SAVE_SLOT_0_TIMELINE", SettingsInfo.saveSlots[0, 1]);
        PlayerPrefs.SetInt("SAVE_SLOT_1_ISFULL", SettingsInfo.saveSlots[1, 0]);
        PlayerPrefs.SetInt("SAVE_SLOT_1_TIMELINE", SettingsInfo.saveSlots[1, 1]);

        // For thumbnail
        if (SettingsInfo.selectedSaveSlot == 0) PlayerPrefs.SetInt("SAVE_SLOT_0_AREA", DetermineArea());
        else if (SettingsInfo.selectedSaveSlot == 1) PlayerPrefs.SetInt("SAVE_SLOT_1_AREA", DetermineArea());
    }

    public void SetContinue(bool FromContinue)
    {
        SettingsInfo.fromContinue = FromContinue;
    }

    private IEnumerator SettingRibbons()
    {
        yield return new WaitForEndOfFrame();

        if (allSaveSlots[0].slotIsFull)
        {
            allSaveSlots[0].ribbon.SetActive(true);
            if (ribbonSprites[allSaveSlots[0].timelineSaved] == null) Debug.Log("Ribbon png is null");
            allSaveSlots[0].SetRibbon(ribbonSprites[allSaveSlots[0].timelineSaved]);
            allSaveSlots[0].SetScreenshot(thumbnailManager.GetScreenshot(
                allSaveSlots[0].timelineSaved,
                PlayerPrefs.GetInt("SAVE_SLOT_0_AREA", 0)
            ));
        }

        if (allSaveSlots[1].slotIsFull)
        {
            allSaveSlots[1].ribbon.SetActive(true);
            if (ribbonSprites[allSaveSlots[1].timelineSaved] == null) Debug.Log("Ribbon png is null");
            allSaveSlots[1].SetRibbon(ribbonSprites[allSaveSlots[1].timelineSaved]);
            allSaveSlots[1].SetScreenshot(thumbnailManager.GetScreenshot(
                allSaveSlots[1].timelineSaved,
                PlayerPrefs.GetInt("SAVE_SLOT_1_AREA", 0)
            ));
        }

        LoadSlotMetadataUI();
    }

    public void SaveSlotMetadata()
    {
        int s = SettingsInfo.selectedSaveSlot;

        PlayerPrefs.SetString($"SAVE_SLOT_{s}_LOCATION", DetermineAreaString());
        PlayerPrefs.SetFloat($"SAVE_SLOT_{s}_PLAYTIME", FindFirstObjectByType<SaveManager>().currentPlaytime);
        PlayerPrefs.SetString($"SAVE_SLOT_{s}_PROGRESSION", "[LAST LOCATION]");

        PlayerPrefs.Save();

        LoadSingleSlotMetadataUI(s);
    }

    private void LoadSlotMetadataUI()
    {
        LoadSingleSlotMetadataUI(0);
        LoadSingleSlotMetadataUI(1);
    }

    // hardcoded slop !! lOLOLOLOL!!! FUCK 1!!!!
    private void LoadSingleSlotMetadataUI(int s)
    {
        TextMeshProUGUI locationLabel = allSaveSlots[s].transform.Find("Location Label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI playtimeLabel = allSaveSlots[s].transform.Find("Playtime Label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI progressionLabel = allSaveSlots[s].transform.Find("Progression Label").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI locationValue = allSaveSlots[s].transform.Find("Location Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI playtimeValue = allSaveSlots[s].transform.Find("Playtime Value").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI progressionValue = allSaveSlots[s].transform.Find("Progression Value").GetComponent<TextMeshProUGUI>();

        if (!allSaveSlots[s].slotIsFull)
        {
            locationLabel.text = "<size=26><i>This is an empty save file</i></size>"; // da default
            playtimeLabel.text = "";
            progressionLabel.text = "";

            locationValue.text = "";
            playtimeValue.text = "";
            progressionValue.text = "";
            return;
        }

        locationLabel.text = "Last Location:";
        playtimeLabel.text = "Total Playtime:";
        progressionLabel.text = "Progression:";

        locationValue.text = PlayerPrefs.GetString($"SAVE_SLOT_{s}_LOCATION", "");
        playtimeValue.text = DeterminePlaytime(PlayerPrefs.GetFloat($"SAVE_SLOT_{s}_PLAYTIME", 0f));
        if (PlayerPrefs.GetFloat($"SAVE_SLOT_{s}_PLAYTIME", 0f) <= 0f) playtimeValue.text = "";
        progressionValue.text = PlayerPrefs.GetString($"SAVE_SLOT_{s}_PROGRESSION", "");
    }

    private int DetermineArea()
    {
        int area = 0; // outpost
        float pos = protag.transform.position.x;

        if (pos > 29) // town
        {
            area = 2;
        } else if (pos > -83) // forest
        {
            area = 1;
        }

        return area;
    }

    private string DetermineAreaString()
    {
        string area = "";

        if (timelineManager.GetCurrentTimeline() == 0) area += "Past ";
        else if (timelineManager.GetCurrentTimeline() == 1) area += "Present ";

        string locationArea = "";
        locationArea = "DG Outpost"; // outpost
        float pos = protag.transform.position.x;

        if (pos > 29) // town
        {
            locationArea = "Town";
        }
        else if (pos > -83) // forest
        {
            locationArea = "Forest";
        }

        area += locationArea;

        return area;
    }

    private string DeterminePlaytime(float secondsFloat)
    {
        int totalSeconds = Mathf.FloorToInt(secondsFloat);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}
