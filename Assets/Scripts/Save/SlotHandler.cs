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
    [SerializeField] GameObject slot3;
    [SerializeField] GameObject slot4;
    [SerializeField] GameObject slot5;
    [SerializeField] GameObject slot6;
    // Sprite references
    [Header("Sprite References")]
    [SerializeField] private Sprite pastRibbon;
    [SerializeField] private Sprite presentRibbon;

    [HideInInspector] public List<SaveSlot> allSaveSlots = new List<SaveSlot>();
    [HideInInspector] public List<Sprite> ribbonSprites = new List<Sprite>();
    private int selectedSlot = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allSaveSlots.Add(slot1.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot2.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot3.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot4.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot5.GetComponent<SaveSlot>());
        allSaveSlots.Add(slot6.GetComponent<SaveSlot>());

        ribbonSprites.Add(pastRibbon);
        ribbonSprites.Add(presentRibbon);

        // TEMPORARY
        allSaveSlots[0].slotIsFull = (PlayerPrefs.GetInt("SAVE_SLOT_ISFULL", 0) != 0);
        allSaveSlots[0].timelineSaved = PlayerPrefs.GetInt("SAVE_SLOT_TIMELINE", 1);
    }

    private void OnEnable()
    {
        StartCoroutine(SettingRibbons());
    }

    public void SelectSlot(int slotNum)
    {
        selectedSlot = slotNum;
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
        allSaveSlots[selectedSlot].ribbon.SetActive(true);
        allSaveSlots[selectedSlot].slotIsFull = true;

        // Set ribbon color
        int currentTimeline = timelineManager.GetCurrentTimeline();
        allSaveSlots[selectedSlot].timelineSaved = currentTimeline;
        allSaveSlots[selectedSlot].SetRibbon(ribbonSprites[currentTimeline]);

        // Set thumbnail
        int area = DetermineArea();
        allSaveSlots[selectedSlot].SetScreenshot(thumbnailManager.GetScreenshot(currentTimeline, area));

        SaveDetails();
    }

    // Saves slot information to player prefs
    public void SaveDetails()
    {
        SettingsInfo.saveSlots[selectedSlot, 0] = allSaveSlots[selectedSlot].slotIsFull ? 1 : 0;
        SettingsInfo.saveSlots[selectedSlot, 1] = allSaveSlots[selectedSlot].timelineSaved;

        // TEMPORARY
        PlayerPrefs.SetInt("SAVE_SLOT_ISFULL", SettingsInfo.saveSlots[selectedSlot, 0]);
        PlayerPrefs.SetInt("SAVE_SLOT_TIMELINE", SettingsInfo.saveSlots[selectedSlot, 1]);
        PlayerPrefs.SetInt("SAVE_SLOT_AREA", DetermineArea());
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
            if (ribbonSprites[allSaveSlots[0].timelineSaved] == null)
            {
                Debug.Log("Ribbon png is null");
            }
            allSaveSlots[0].SetRibbon(ribbonSprites[allSaveSlots[0].timelineSaved]);
            allSaveSlots[0].SetScreenshot(thumbnailManager.GetScreenshot(
                allSaveSlots[0].timelineSaved,
                PlayerPrefs.GetInt("SAVE_SLOT_AREA", 0)
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
