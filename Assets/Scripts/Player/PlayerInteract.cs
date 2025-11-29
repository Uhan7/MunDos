using NaughtyAttributes;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] private PlayerMove moveScript;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private GameObject inventorySlots;
    [SerializeField] private InventoryUI inventoryUI;

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
    [HideInInspector] public bool canInput;

    [HideInInspector] public bool HasActiveItem;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool isZoomed;
    [HideInInspector] public int activeObjectIndex;

    private void Awake()
    {
        moveScript = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        playerItemIndex = 0;
        activeObjectIndex = 5;
        currentItemData = itemDatas[playerItemIndex];
        hasCheckedInventory = false;
        canInput = true;
    }

    private void Update()
    {
        if (!canInput)
        {
            return;
        }
        else if ((Input.GetKeyDown(interactKey) || Input.GetKeyDown(otherInteractKey)) && !Input.GetKey(switchTimelineKey))
        {
            willUpdate = true;
            if (nearbyEnvi != null) Interact();
            if (nearbyItem != null) PickupItem();
        }
        if (inventorySlots.activeInHierarchy)
        {
            hasCheckedInventory = false;
            if (Input.GetKeyDown(item1Key)) KeyPress(0);
            else if (Input.GetKeyDown(item2Key)) KeyPress(1);
            else if (Input.GetKeyDown(item3Key)) KeyPress(2);
            else if (Input.GetKeyDown(item4Key)) KeyPress(3);
            else if (Input.GetKeyDown(item5Key)) KeyPress(4);
        }

        else if (!inventorySlots.activeInHierarchy && !hasCheckedInventory)
        {
            SelectItem(5);
            hasCheckedInventory = true;
            isZoomed = isSelected = false;
            ResetStates();
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
        if (!col.CompareTag("NPC") &&
            !col.CompareTag("Envi") &&
            !col.CompareTag("Item")) return;

        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Envi":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                nearbyEnvi = col.gameObject;

                if (moveScript.canMove && moveScript.canInput) SetInteractableObjectOutline(nearbyEnvi, true);
                else SetInteractableObjectOutline(nearbyEnvi, false);
                break;

            case "Item":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                if (nearbyItem != null) SetInteractableObjectOutline(nearbyItem, false);
                nearbyItem = col.gameObject;

                if (moveScript.canMove && moveScript.canInput) SetInteractableObjectOutline(nearbyItem, true);
                else SetInteractableObjectOutline(nearbyItem, false);
                break;

            default:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("NPC") &&
            !col.CompareTag("Envi") &&
            !col.CompareTag("Item")) return;

        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Envi":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                nearbyEnvi = col.gameObject;

                if (moveScript.canMove && moveScript.canInput) SetInteractableObjectOutline(nearbyEnvi, true);
                else SetInteractableObjectOutline(nearbyEnvi, false);
                break;

            case "Item":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                if (nearbyItem != null) SetInteractableObjectOutline(nearbyItem, false);
                nearbyItem = col.gameObject;

                if (moveScript.canMove && moveScript.canInput && nearbyEnvi == null) SetInteractableObjectOutline(nearbyItem, true);
                else SetInteractableObjectOutline(nearbyItem, false);
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

    public void ToggleStates(int value) // This is for the inventory
    {
        if (!canInput) return;
        if (!SelectValidItem(value)) return;
        
        //value + 1 to offset Background
        InventorySlot inventorySlot = inventorySlots.transform.GetChild(value + 1).GetComponent<InventorySlot>();
        if (inventorySlot == null) Debug.LogError($"{this.gameObject.name}'s inventoryslot not correctly set");

        //Select unselected item
        if (!isZoomed && !isSelected)
        {
            if (gameManagerScript.isFocusing) return; // This is to not layer the zooms

            isSelected = true;
            activeObjectIndex = value;
            WillUpdate(true);
        }

        //Zoom in on valid selected item
        else if (!isZoomed && isSelected && value == activeObjectIndex)
        {
            if (gameManagerScript.isFocusing) return; // This is to not layer the zooms

            inventorySlot.SetZoomObject();
            inventorySlot.SetZoomState(true);
            inventorySlot.OnZoom();
            isZoomed = true;
            WillUpdate(true);
        }

        //Cancel Zoom in and select other item
        else if (!isZoomed && isSelected && value != activeObjectIndex)
        {
            if (gameManagerScript.isFocusing) return; // This is to not layer the zooms

            inventorySlot.SetZoomObject();
            inventorySlot.SetZoomState(false);
            inventorySlot.OnZoom();
            isZoomed = false;
            activeObjectIndex = value;
            WillUpdate(true);
        }

        //Zoom out of same item
        else if (isZoomed && isSelected && value == activeObjectIndex)
        {
            inventorySlot.SetZoomState(false);
            isZoomed = false;
            isSelected = false;
            GetComponent<Animator>().Play("object_fade_out_half");
            WillUpdate(true);
        }

        //Zoom out then select different item
        else if (isZoomed && isSelected && value != activeObjectIndex)// Zoom out of item, click another item
        {
            InventorySlot previous = inventorySlots.transform.GetChild(activeObjectIndex + 1).GetComponent<InventorySlot>();
            if (previous == null) Debug.LogError($"{this.gameObject.name}'s previous not correctly set");

            previous.SetZoomState(false);
            //isSelected = false;
            isZoomed = false;
            activeObjectIndex = value;
            WillUpdate(true);
        }
    }

    
    // Helper Functions --------------------------------------------------------

    void Interact()
    {
        if ((nearbyEnvi == null) || !moveScript.canInput) return;

        if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);

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

        // Jiggles inventory button
        inventoryUI.FlashButton();
    }

    public void ClearItem(string playeritemName)
    {
        for (int i = 0; i < 5; i++)
        {
            if (itemDatas[i].itemName == playeritemName)
            {
                itemDatas[i].itemName = "";
                itemDatas[i].itemSprite = null;
                itemDatas[i].objectsToInteractWith = null;
            }
        }

        willUpdate = true;
    }

    public void GiveItem(Item item)
    {
        playerItemIndex = FindEmptySlot();
        itemDatas[playerItemIndex] = item.GetData();
        SetCurrentItem();
        item.PickedUp();
    }
    //public bool TryRemoveItem(Item item)
    //{
    //    int index = DoesItemExist(item);
    //    if (index <= -1 || index > 5) return false;

    //    itemDatas[index] = null;
    //    if (index  >= itemDatas.Length - 1) return true;
    //    for (int i = index + 1; i < itemDatas.Length - 1; i++)
    //    {
    //        itemDatas[i] = itemDatas[i + 1];
    //    }
    //    itemDatas[itemDatas.Length - 1] = null;
    //    return true;
    //}
    public void TryRemoveItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning($"TryRemoveItem: {item} is null");
            return;
        }
        int index = HasItemName(item);
        if (index <= -1 || index > itemDatas.Length || index >= 5) return;

        
        
        
        isSelected = isZoomed = false;
        playerItemIndex = 5;

        InventorySlot activeItem = inventorySlots.transform.GetChild(activeObjectIndex + 1).GetComponent<InventorySlot>();
        if (activeItem) activeItem.SetZoomState(false);

        
        itemDatas[index].itemSprite = null;

        itemDatas[index].itemName = "";
        WillUpdate(true);
        //if (index >= itemDatas.Length - 1) return;
        //for (int i = index + 1; i < itemDatas.Length - 1; i++)
        //{
        //    itemDatas[i] = itemDatas[i + 1];
        //}
        //itemDatas[itemDatas.Length - 1] = null;
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

    public int HasItemName(Item item)
    {
        int index = 0;
        foreach (ItemData itemData in itemDatas)
        {
            if (item.GetData().itemName == itemData.itemName) return index;
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
        isSelected = true;
        playerItemIndex = index;
        SetCurrentItem();
        return true;
    }
    void KeyPress(int index)
    {
        if (playerItemIndex == index && isSelected)
        {
            SelectItem(5);
            ResetStates();
        }
        else
        {
            SelectValidItem(index);
        }
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

    public void ResetStates()
    {
        if (activeObjectIndex == 5) return;
        InventorySlot activeItem = inventorySlots.transform.GetChild(activeObjectIndex + 1).GetComponent<InventorySlot>();
        if (activeItem) activeItem.SetZoomState(false);
        isSelected = false;
        isZoomed = false;
        activeObjectIndex = 5;
        WillUpdate(true);
    }
}
