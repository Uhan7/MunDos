using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    private GameObject interactedObject;

    [SerializeField] private GameObject interactableFeedbackObject;

    private GameObject nearbyItem;
    [SerializeField] private ItemData currentItemData;

    private void Update()
    {
        InteractableFeedback();
        if (Input.GetKeyDown(interactKey))
        {
            if (interactedObject != null) Interact();
            if (nearbyItem != null) PickupItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Environment":
                interactedObject = col.gameObject;
                break;

            case "Item":
                nearbyItem = col.gameObject;
                break;

            default:

                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == interactedObject) interactedObject = null;
    }

    // Update Functions --------------------------------------------------------

    void InteractableFeedback()
    {
        interactableFeedbackObject.SetActive(interactedObject != null || nearbyItem != null);
    }

    void Interact()
    {
        if (interactedObject == null) return;

        if (currentItemData.itemName == "") interactedObject.GetComponent<InteractableObject>().Interact();
        else
        {
            if (currentItemData.objectToInteractWith == interactedObject) interactedObject.GetComponent<InteractableObject>().ItemInteract(true);
            else interactedObject.GetComponent<InteractableObject>().ItemInteract(false);
        }
    }

    void PickupItem()
    {
        Item actualItem = nearbyItem.GetComponent<Item>();

        currentItemData = actualItem.GetData();

        Destroy(nearbyItem);
    }
}
