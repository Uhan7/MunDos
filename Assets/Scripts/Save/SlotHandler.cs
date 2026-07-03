using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
