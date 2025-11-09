using UnityEngine;
using NaughtyAttributes;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] private PlayerMove moveScript;
    [SerializeField] private GameObject inventorySlots;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode switchTimelineKey;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private KeyCode otherInteractKey;
    [SerializeField] private KeyCode previousItemKey;
    [SerializeField] private KeyCode nextItemKey;
    [SerializeField] private KeyCode item1Key;
    [SerializeField] private KeyCode item2Key;
    [SerializeField] private KeyCode item3Key;
    [SerializeField] private KeyCode item4Key;
    [SerializeField] private KeyCode item5Key;

    [Header("Nearby Objects")]
    [HideInInspector] private GameObject nearbyEnvi;
    [HideInInspector] private GameObject nearbyItem;

    [Header("Playeritems Data")]
    [HideInInspector] public ItemData currentItemData; // Used in PlayeritemsManager
    [ReadOnly] public ItemData[] itemDatas; // Used in PlayeritemsManager
    [HideInInspector] public int playerItemIndex; // Used in PlayeritemsManager

    [Header("Flags")]
    [SerializeField] private bool willUpdate;
    [SerializeField] public bool isEmpty;
    [SerializeField] private bool hasCheckedInventory;

    private void Awake()
    {
        moveScript = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        playerItemIndex = 0;
        currentItemData = itemDatas[playerItemIndex];
        hasCheckedInventory = false;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(interactKey) || Input.GetKeyDown(otherInteractKey)) && !Input.GetKey(switchTimelineKey))
        {
            willUpdate = true;
            if (nearbyEnvi != null) Interact();
            if (nearbyItem != null) PickupItem();
        }
        if (inventorySlots.activeInHierarchy)
        {
            hasCheckedInventory = false;
            if (Input.GetKeyDown(item1Key)) SelectValidItem(0);
            if (Input.GetKeyDown(item2Key)) SelectValidItem(1);
            if (Input.GetKeyDown(item3Key)) SelectValidItem(2);
            if (Input.GetKeyDown(item4Key)) SelectValidItem(3);
            if (Input.GetKeyDown(item5Key)) SelectValidItem(4);
        }

        else if (!inventorySlots.activeInHierarchy && !hasCheckedInventory)
        {
            Debug.Log($"not active in heirarchy");
            SelectItem(5);
            hasCheckedInventory = true;
            return;
        }

        /*
        if (Input.GetKeyDown(previousItemKey)) SelectItem("PREVIOUS");
        if (Input.GetKeyDown(nextItemKey)) SelectItem("NEXT");
        */

        // Debugs can go here ---

        // awooga

        // End of debugs ---
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Envi":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                nearbyEnvi = col.gameObject;
                if (moveScript.canMove) SetInteractableObjectOutline(nearbyEnvi, true);
                break;

            case "Item":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                if (nearbyItem != null) SetInteractableObjectOutline(nearbyItem, false);
                nearbyItem = col.gameObject;

                if (moveScript.canInput) SetInteractableObjectOutline(nearbyItem, true);
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
        if (nearbyItem != null) SetInteractableObjectOutline(nearbyItem, false);

        if (col.gameObject == nearbyEnvi) nearbyEnvi = null;
        if (col.gameObject == nearbyItem) nearbyItem = null;
    }
    public void WillUpdate(bool value)
    {
        willUpdate = value;
    }
    public bool GetWillUpdate()
    {
        return willUpdate;
    }

    // Helper Functions --------------------------------------------------------

    void Interact()
    {
        if ((nearbyEnvi == null) || !moveScript.canInput) return;

        if (currentItemData.itemName == "") nearbyEnvi.GetComponent<InteractableObject>().Interact();
        else
        {
            bool match = false;

            foreach (GameObject obj in currentItemData.objectsToInteractWith)
            {
                if (obj == nearbyEnvi)
                {
                    nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(true);
                    match = true;
                }
            }

            if (!match)
            {
                nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(false);
            }
        }

        if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
    }

    public int FindEmptySlot()
    {
        int index = playerItemIndex;

        for (int i = 0; i < itemDatas.Length - 1; i++)
        {
            if (itemDatas[index].itemName != "")
            {
                index++;
                if (index >= itemDatas.Length) index = 0;
            }
        }

        return index;
    }

    void PickupItem()
    {
        if ((nearbyItem == null) || !moveScript.canMove) return;

        Item actualItem = nearbyItem.GetComponent<Item>();

        playerItemIndex = 0;
        playerItemIndex = FindEmptySlot();

        itemDatas[playerItemIndex] = actualItem.GetData();
        SetCurrentItem();

        actualItem.PickedUp();
    }
    public void GiveItem(Item item)
    {
        playerItemIndex = FindEmptySlot();
        itemDatas[playerItemIndex] = item.GetData();
        SetCurrentItem();
        item.PickedUp();
    }
    public bool TryRemoveItem(Item item)
    {
        int index = DoesItemExist(item);
        if (index <= -1) return false;

        itemDatas[index] = null;
        if (index + 1 >= itemDatas.Length - 1) return true;
        for (int i = index + 1; i < itemDatas.Length - 1; i++)
        {
            if (itemDatas[index] == null) continue;
            itemDatas[index] = itemDatas[index + 1];
        }
        itemDatas[itemDatas.Length - 1] = null;
        return true;
    }
    public int DoesItemExist(Item item)
    {
        int index = 0;
        foreach (ItemData itemData in itemDatas)
        {
            if (item.GetData() == itemData) return index;
            index++;
        }
        return -1;
    }
    void SetCurrentItem()
    {
        currentItemData = itemDatas[playerItemIndex];
        Debug.Log($"CurItemData at {playerItemIndex} is {itemDatas[playerItemIndex].itemName}");
    }

    public void SelectItem(int index)
    {

        playerItemIndex = index;
        SetCurrentItem();
    }

    public void SelectValidItem(int index)
    {
        
        if (itemDatas[index].itemName == "")
        {
            Debug.Log($"Error CurItemData empty");
            return;
        }
        playerItemIndex = index;
        SetCurrentItem();
    }

    void SelectItem(string value)
    {
        switch (value)
        {
            case "PREVIOUS":
                playerItemIndex--;
                if (playerItemIndex < 0) playerItemIndex = itemDatas.Length - 1;
                break;

            case "NEXT":
                playerItemIndex++;
                if (playerItemIndex >= itemDatas.Length) playerItemIndex = 0;
                break;

            default:
                break;
        }

        SetCurrentItem();
    }
    
    void SetInteractableObjectOutline(GameObject obj, bool var)
    {
        if (!moveScript.canInput) return;

        SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        Sprite objOutlinedSprite = obj.GetComponent<InteractableObject>().outlinedSprite;
        Sprite objNormalSprite = obj.GetComponent<InteractableObject>().normalSprite;

        if (var == true)
        {
            if (objOutlinedSprite != objNormalSprite) objSpriteRenderer.sprite = objOutlinedSprite;
            else obj.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        }
        else
        {
            if (objOutlinedSprite != objNormalSprite) objSpriteRenderer.sprite = objNormalSprite;
            else obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

}
