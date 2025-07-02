using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject interactableFeedbackObject;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private KeyCode swapItemKey;

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

        if (Input.GetKeyDown(swapItemKey))
        {
            playerItemIndex++;
            if (playerItemIndex >= itemDatas.Length) playerItemIndex = 0;

            currentItemData = itemDatas[playerItemIndex];
        }
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

    void PickupItem()
    {
        Item actualItem = nearbyItem.GetComponent<Item>();

        for (int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[playerItemIndex].itemName != "")
            {
                playerItemIndex++;
                if (playerItemIndex >= itemDatas.Length) playerItemIndex = 0;
            }
        }

        itemDatas[playerItemIndex] = actualItem.GetData();
        //currentItemData = actualItem.GetData();
        currentItemData = itemDatas[playerItemIndex];

        actualItem.PickedUp();
    }
}
