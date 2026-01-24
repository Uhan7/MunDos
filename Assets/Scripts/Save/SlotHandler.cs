using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotHandler : MonoBehaviour
{
    [SerializeField] TimelineManager timelineManager;
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

        int currentTimeline = timelineManager.GetCurrentTimeline();
        allSaveSlots[selectedSlot].timelineSaved = currentTimeline;
        allSaveSlots[selectedSlot].SetRibbon(ribbonSprites[currentTimeline]);

        // SCREEN SCREENSHOT HERE

        // ACTUAL SAVE FEATURE HERE
    }
}
