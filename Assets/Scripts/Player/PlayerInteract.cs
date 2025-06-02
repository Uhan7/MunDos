using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    private GameObject nearbyObject;

    [SerializeField] private GameObject interactableFeedbackObject;

    private GameObject nearbyItem;
    [SerializeField] private ItemData currentItemData;

    private void Update()
    {
        InteractableFeedback();
        if (Input.GetKeyDown(interactKey))
        {
            if (nearbyObject != null) Interact();
            if (nearbyItem != null) PickupItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "NPC":
            case "Environment":
                nearbyObject = col.gameObject;
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
        if (col.gameObject == nearbyObject) nearbyObject = null;
        if (col.gameObject == nearbyItem) nearbyItem = null;
    }

    // Update Functions --------------------------------------------------------

    void InteractableFeedback()
    {
        interactableFeedbackObject.SetActive(nearbyObject != null || nearbyItem != null);
    }

    void Interact()
    {
        if (nearbyObject == null) return;

        if (currentItemData.itemName == "") nearbyObject.GetComponent<InteractableObject>().Interact();
        else
        {

            for (int i = 0; i < currentItemData.objectToInteractWith.)
            //foreach (GameObject obj in currentItemData.objectToInteractWith.)
            //{
            //
            //} 

            if (currentItemData.objectToInteractWith == nearbyObject) nearbyObject.GetComponent<InteractableObject>().ItemInteract(true);
            else nearbyObject.GetComponent<InteractableObject>().ItemInteract(false);
        }
    }

    void PickupItem()
    {
        Item actualItem = nearbyItem.GetComponent<Item>();

        currentItemData = actualItem.GetData();

        Destroy(nearbyItem);
    }
}
