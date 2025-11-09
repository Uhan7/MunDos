using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    [HideInInspector] private bool willUpdate;
    [HideInInspector] public bool isEmpty;
    [HideInInspector] private bool hasCheckedInventory;

    [HideInInspector] public bool HasActiveItem;
    [HideInInspector] public bool isClicked;
    [HideInInspector] public bool isZoomed;
    [HideInInspector] public int activeObjectIndex;

    private void Awake()
    {
        moveScript = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        playerItemIndex = 0;
        activeObjectIndex = -1;
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
            else if (Input.GetKeyDown(item2Key)) SelectValidItem(1);
            else if (Input.GetKeyDown(item3Key)) SelectValidItem(2);
            else if (Input.GetKeyDown(item4Key)) SelectValidItem(3);
            else if (Input.GetKeyDown(item5Key)) SelectValidItem(4);
        }

        else if (!inventorySlots.activeInHierarchy && !hasCheckedInventory)
        {
            SelectItem(5);
            hasCheckedInventory = true;
            isZoomed = isClicked = false;
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

    public void ToggleStates(int value)
    {
        //value + 1 to offset Background
        InventorySlot inventorySlot = inventorySlots.transform.GetChild(value + 1).GetComponent<InventorySlot>();
        if (inventorySlot == null) Debug.LogError($"{this.gameObject.name}'s inventoryslot not correctly set");
        if (!SelectValidItem(value)) return;

        //Select unselected item
        if (!isZoomed && !isClicked)
        {
            isClicked = true;
            activeObjectIndex = value;
            WillUpdate(true);
        }

        //Zoom in on valid selected item
        else if (!isZoomed && isClicked && value == activeObjectIndex)// Zoom in on valid item
        {
            inventorySlot.SetZoomObject();
            inventorySlot.SetZoomState(true);
            inventorySlot.OnZoom();
            isZoomed = true;
            WillUpdate(true);
        }

        //Zoom out of same item
        else if (isZoomed && isClicked && (value == activeObjectIndex))// Zoom out of item
        {
            inventorySlot.SetZoomState(false);
            isZoomed = false;
            //isClicked = false;
            WillUpdate(true);
        }

        //Zoom out when select different item
        else if (isZoomed && isClicked && (value != activeObjectIndex))// Zoom out of item, click another item
        {
            InventorySlot previous = inventorySlots.transform.GetChild(activeObjectIndex + 1).GetComponent<InventorySlot>();
            if (previous == null) Debug.LogError($"{this.gameObject.name}'s previous not correctly set");

            previous.SetZoomState(false);
            isClicked = false;
            isZoomed = false;
            WillUpdate(true);
        }
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
                    if (obj.GetComponent<Oil>() != null)
                    {
                        obj.GetComponent<Oil>().ItemInteract(currentItemData.itemName);
                        match = true;
                    }

                    else
                    {
                        nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(true);
                        match = true;
                    }
                }
            }

            if (!match) nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(false);
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
        SelectItem(5);
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
        willUpdate = true;
    }

    

    public void SelectItem(int index)
    {

        playerItemIndex = index;
        SetCurrentItem();
    }

    public bool SelectValidItem(int index)
    {
        if (itemDatas[index].itemName == "")
        {
            return false;
        }
        HasActiveItem = true;
        playerItemIndex = index;
        SetCurrentItem();
        return true;
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
