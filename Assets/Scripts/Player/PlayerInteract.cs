using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
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

    [Header("PlayerItems")]
    [SerializeField] private ItemData currentItemData;
    [SerializeField] private ItemData[] itemDatas;
    [SerializeField] private int playerItemIndex;

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
                nearbyEnvi = col.gameObject;
                break;

            case "Item":
                nearbyItem = col.gameObject;
                break;

            default:
                break;
        }

        Debug.Log(nearbyEnvi);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == nearbyEnvi) nearbyEnvi = null;
        if (col.gameObject == nearbyItem) nearbyItem = null;
    }

    // Update Functions --------------------------------------------------------

    void InteractableFeedback()
    {
        interactableFeedbackObject.SetActive(nearbyEnvi != null || nearbyItem != null);
    }

    void Interact()
    {
        if (nearbyEnvi == null) return;

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
}
