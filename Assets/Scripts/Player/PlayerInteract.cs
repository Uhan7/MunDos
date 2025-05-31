using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    private GameObject interactedObject;

    [SerializeField] private GameObject interactableFeedbackObject;

    private void Update()
    {

        InteractableFeedback();
        if (Input.GetKeyDown(interactKey))
        {
            Interact();
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
        interactableFeedbackObject.SetActive(interactedObject != null);
    }

    void Interact()
    {
        if (interactedObject == null) return;

        Debug.Log("Interacted with" + interactedObject);
    }
}
