using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    private PlayerMove moveScript;

    [Header("GameObjects")]
    [SerializeField] private GameObject interactableFeedbackObject;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode interactKey;
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
    [HideInInspector] public ItemData[] itemDatas; // Used in PlayeritemsManager
    [HideInInspector] public int playerItemIndex; // Used in PlayeritemsManager

    private void Awake()
    {
        moveScript = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        playerItemIndex = 0;
        currentItemData = itemDatas[playerItemIndex];
    }

    private void Update()
    {
        InteractableFeedback();

        if (Input.GetKeyDown(interactKey))
        {
            if (nearbyEnvi != null) Interact();
            if (nearbyItem != null) PickupItem();
        }

        if (Input.GetKeyDown(previousItemKey))
        {
            SelectItem("PREVIOUS");
        }

        if (Input.GetKeyDown(nextItemKey))
        {
            SelectItem("NEXT");
        }

        if (Input.GetKeyDown(item1Key)) SelectItem(0);
        if (Input.GetKeyDown(item2Key)) SelectItem(1);
        if (Input.GetKeyDown(item3Key)) SelectItem(2);
        if (Input.GetKeyDown(item4Key)) SelectItem(3);
        if (Input.GetKeyDown(item5Key)) SelectItem(4);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Envi":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                nearbyEnvi = col.gameObject;
                SetInteractableObjectOutline(nearbyEnvi, true);
                break;

            case "Item":
                if (nearbyEnvi != null) SetInteractableObjectOutline(nearbyEnvi, false);
                if (nearbyItem != null) SetInteractableObjectOutline(nearbyItem, false);
                nearbyItem = col.gameObject;
                SetInteractableObjectOutline(nearbyItem, true);
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

    // Helper Functions --------------------------------------------------------

    void InteractableFeedback()
    {
        if (!moveScript.canMove)
        {
            interactableFeedbackObject.SetActive(false); // change to make obj outline false
            //SetInteractableObjectOutline(false);
            return;
        }
        // Set object outline to true of nearbyItem if it exists,
        // else, set object outline of nearbyEnvi true instead,
        // else, none of em are true (it should only be one at a time)
        interactableFeedbackObject.SetActive(nearbyEnvi != null || nearbyItem != null); // change to be outline
        //SetInteractableObjectOutline(nearbyEnvi != null);
    }

    void Interact()
    {
        if ((nearbyEnvi == null) || !moveScript.canMove) return;

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
    }

    int FindEmptySlot()
    {
        int index = playerItemIndex;

        for (int i = 0; i < itemDatas.Length; i++)
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

        playerItemIndex = FindEmptySlot();

        itemDatas[playerItemIndex] = actualItem.GetData();
        SetCurrentItem();

        actualItem.PickedUp();
    }

    void SetCurrentItem()
    {
        currentItemData = itemDatas[playerItemIndex];
    }

    void SelectItem(int index)
    {
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
        if (var == true) obj.GetComponent<InteractableObject>().spriteRenderer.sprite = obj.GetComponent<InteractableObject>().outlinedSprite;
        else obj.GetComponent<InteractableObject>().spriteRenderer.sprite = obj.GetComponent<InteractableObject>().normalSprite;
    }
}
