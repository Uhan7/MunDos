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
    [SerializeField] private ItemData item1Data;
    [SerializeField] private ItemData item2Data;
    [SerializeField] private int playerItemIndex;

    private void Start()
    {
        playerItemIndex = 1;
        currentItemData = item1Data;
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
            if (playerItemIndex == 1) playerItemIndex = 2;
            else if (playerItemIndex == 2) playerItemIndex = 1;

            if (playerItemIndex == 1) currentItemData = item1Data;
            if (playerItemIndex == 2) currentItemData = item2Data;
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

        if (playerItemIndex == 1) item1Data = actualItem.GetData();
        if (playerItemIndex == 2) item2Data = actualItem.GetData();
        //currentItemData = actualItem.GetData();
        if (playerItemIndex == 1) currentItemData = item1Data;
        if (playerItemIndex == 2) currentItemData = item2Data;

        actualItem.PickedUp();
    }
}
