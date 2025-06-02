using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [SerializeField] private bool itemInteractable;

    [SerializeField] private GameObject toActivateOnInteract;
    [SerializeField] private GameObject toDeactivateOnInteract;

    public void Interact()
    {
        Debug.Log(gameObject + " did something.");

        if (toActivateOnInteract != null) toActivateOnInteract.SetActive(true);
        if (toDeactivateOnInteract != null) toDeactivateOnInteract.SetActive(false);
    }

    public void ItemInteract(bool var)
    {
        if (!itemInteractable)
        {
            Interact();
            return;
        }

        if (var == false)
        {
            Debug.Log("That didn't do anything.");
        }
        else
        {
            Debug.Log("Correct Interaction");
        }
    }
}
