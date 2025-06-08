using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    private GameObject nearbyEnvi;

    [SerializeField] private GameObject interactableFeedbackObject;

    private GameObject nearbyItem;
    [SerializeField] private ItemData currentItemData;

    private void Update()
    {
        InteractableFeedback();
        if (Input.GetKeyDown(interactKey))
        {
            if (nearbyEnvi != null) Interact();
            if (nearbyItem != null) PickupItem();
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

        if (currentItemData == null) nearbyEnvi.GetComponent<InteractableObject>().Interact();
        else
        {
            bool match = false;

            foreach (GameObject obj in currentItemData.objectsToInteractWith)
            {
                if (obj == nearbyEnvi) nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(true);
                match = true;
            }

            if (!match) nearbyEnvi.GetComponent<InteractableObject>().ItemInteract(false);
        }
    }

    void PickupItem()
    {
        Item actualItem = nearbyItem.GetComponent<Item>();

        currentItemData = actualItem.GetData();

        Destroy(nearbyItem);
    }
}
