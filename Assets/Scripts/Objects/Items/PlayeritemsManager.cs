using UnityEngine;
using UnityEngine.UI;

public class PlayeritemsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInteract pastProtagScript;
    [SerializeField] private PlayerInteract presentProtagScript;
    [SerializeField] private InventoryUI pastInventoryUI;
    [SerializeField] private InventoryUI presentInventoryUI;
    [SerializeField] private GameObject[] pastPlayeritemSlots;
    [SerializeField] private GameObject[] presentPlayeritemSlots;
    


    private void Awake()
    {
        //InitialCache();
    }

    private void Start()
    {
        if (pastProtagScript == null) Debug.LogError($"{this.name}'s pastProtagScript null");
        if (presentProtagScript == null) Debug.LogError($"{this.name}'s presentProtagScript null");
        if (pastInventoryUI == null) Debug.LogError($"{this.name}'s pastInventoryUI null");
        if (presentInventoryUI == null) Debug.LogError($"{this.name}'s presentInventoryUI null");
        InitialValues();
    }

    // THIS IS TEMPORARY - don't put it in Update() since that's too expensive, wait for broadcast manager for dis.
    private void Update()
    {
        if (WillUpdatePast())
        {
            CheckInventorySlots(pastProtagScript, pastPlayeritemSlots);
            pastInventoryUI.WillUpdate = false;
        }
        else if (willUpdatePresent())
        {
            CheckInventorySlots(presentProtagScript, presentPlayeritemSlots);
            presentInventoryUI.WillUpdate = false;
        }

        // TEMP ---
    }


    
    // Helper Functions --------------------------------------------------------

    bool WillUpdatePast()
    {

        if (pastProtagScript.GetWillUpdate() || pastInventoryUI.WillUpdate)
        {
            Debug.Log($"past will update {pastInventoryUI.WillUpdate}");
            return true;
        }
        return false;
    }
    bool willUpdatePresent()
    {
        if (presentProtagScript.GetWillUpdate() || presentInventoryUI.WillUpdate) return true;
        return false;
    }

    void CheckInventorySlots(PlayerInteract player, GameObject[] itemSlots)
    {
        player.WillUpdate(false);
        //if (!player.gameObject.activeInHierarchy) return;
        for (int i = 0; i < player.itemDatas.Length - 1; i++)
        {
            if (player.itemDatas[i].itemName != "")
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = player.itemDatas[i].itemSprite;
            }
            if (player.playerItemIndex == i)
            {
                Debug.Log($"updating inven. Item {player.playerItemIndex}: {itemSlots[i].GetComponent<InventorySlot>().name} is selected");
                itemSlots[i].GetComponent<InventorySlot>().IsSelected(true);
            }
            else itemSlots[i].GetComponent<InventorySlot>().IsSelected(false);
        }
        

    }

    void SetUpdate(bool value)
    {

    }
    void InitialValues()
    {
        //foreach (GameObject slot in pastPlayeritemSlots) slot.GetComponent<InventorySlot>().IsSelected(false); ;
        //foreach (GameObject slot in presentPlayeritemSlots) slot.GetComponent<InventorySlot>().IsSelected(false); ;

        //ERM
    }
}
