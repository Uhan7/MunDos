using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    private GameObject interactedObject;

    [SerializeField] private GameObject interactableFeedbackObject;

    private GameObject nearbyItem;
    [SerializeField] private ItemData currentItemData;
    [SerializeField] private string test;

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
                interactedObject.GetComponent<InteractableObject>().Interact();
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

        Debug.Log("Interacted with " + interactedObject.GetComponentInParent<GameObject>().name);
    }

    void PickupItem()
    {
        Item actualItem = nearbyItem.GetComponent<Item>();

        currentItemData = actualItem.GetData();

        Destroy(nearbyItem);
    }
}
