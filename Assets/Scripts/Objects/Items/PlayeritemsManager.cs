using UnityEngine;
using UnityEngine.UI;

public class PlayeritemsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInteract pastProtagScript;
    [SerializeField] private PlayerInteract presentProtagScript;
    [SerializeField] private GameObject[] pastPlayeritemSlots;
    [SerializeField] private GameObject[] presentPlayeritemSlots;

    private void Awake()
    {
        //InitialCache();
    }

    private void Start()
    {
        InitialValues();
    }

    // THIS IS TEMPORARY - don't put it in Update() since that's too expensive, wait for broadcast manager for dis.
    private void Update()
    {
        // TEMP ---

        //if (pastProtagScript.gameObject.activeInHierarchy) {
        //    for (int i = 0; i < pastProtagScript.itemDatas.Length; i++)
        //    {
        //        if (pastProtagScript.itemDatas[i].itemName != "") pastPlayeritemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = pastProtagScript.itemDatas[i].itemSprite;

        //        if (pastProtagScript.playerItemIndex == i) pastPlayeritemSlots[i].GetComponent<InventorySlot>().IsSelected(true);
        //        else pastPlayeritemSlots[i].GetComponent<InventorySlot>().IsSelected(false);
        //    }
        //}

        //if (presentProtagScript.gameObject.activeInHierarchy) {
        //    for (int i = 0; i < presentProtagScript.itemDatas.Length; i++)
        //    {
        //        if (presentProtagScript.itemDatas[i].itemName != "") presentPlayeritemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = presentProtagScript.itemDatas[i].itemSprite;

        //        if (presentProtagScript.playerItemIndex == i) presentPlayeritemSlots[i].GetComponent<InventorySlot>().IsSelected(true);
        //        else presentPlayeritemSlots[i].GetComponent<InventorySlot>().IsSelected(false);
        //    }
        //}

        if (pastProtagScript.GetWillUpdate()) CheckInventorySlots(pastProtagScript, pastPlayeritemSlots);
        if (presentProtagScript.GetWillUpdate()) CheckInventorySlots(presentProtagScript, presentPlayeritemSlots);

        // TEMP ---
    }


    
    // Helper Functions --------------------------------------------------------

    void CheckInventorySlots(PlayerInteract player, GameObject[] itemSlots)
    {
        if (!player.gameObject.activeInHierarchy) return;
        for (int i = 0; i < player.itemDatas.Length - 1; i++)
        {
            if (player.itemDatas[i].itemName != "")
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = player.itemDatas[i].itemSprite;
            }
            if (player.playerItemIndex == i) itemSlots[i].GetComponent<InventorySlot>().IsSelected(true);
            else itemSlots[i].GetComponent<InventorySlot>().IsSelected(false);
        }
        player.WillUpdate(false);
    }
    void InitialValues()
    {
        //foreach (GameObject slot in pastPlayeritemSlots) slot.GetComponent<InventorySlot>().IsSelected(false); ;
        //foreach (GameObject slot in presentPlayeritemSlots) slot.GetComponent<InventorySlot>().IsSelected(false); ;

        //ERM
    }
}
