using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTimelineSyncer : MonoBehaviour
{
    [SerializeField] SlotHandler slotHandler;
    [SerializeField] GameObject slot1;
    [SerializeField] GameObject slot2;
    [SerializeField] GameObject slot3;
    [SerializeField] GameObject slot4;
    [SerializeField] GameObject slot5;
    [SerializeField] GameObject slot6;

    private List<SaveSlot> allSaveSlots = new List<SaveSlot>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //allSaveSlots.Add(slot1.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot2.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot3.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot4.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot5.GetComponent<SaveSlot>());
        //allSaveSlots.Add(slot6.GetComponent<SaveSlot>());

        slotHandler = GetComponent<SlotHandler>();
    }
    private IEnumerator SyncSlotUI()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < allSaveSlots.Count; i++)
        {
            if (slotHandler.allSaveSlots[i].slotIsFull)
            {
                allSaveSlots[i].ribbon.SetActive(true);
                // SCREENSHOT SYNC SOMEHOW
                allSaveSlots[i].SetRibbon(slotHandler.ribbonSprites[slotHandler.allSaveSlots[i].timelineSaved]);
            } else
            {
                allSaveSlots[i].ribbon.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        //StartCoroutine(SyncSlotUI());
    }

    public void Sync()
    {
        //StartCoroutine(SyncSlotUI());
    }
}