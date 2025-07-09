using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public bool itemInteractable; // Used in PlayerInteract.cs

    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [SerializeField] private GameObject[] toActivateOnValidInteract;
    [SerializeField] private GameObject[] toDeactivateOnValidInteract;

    [SerializeField] private bool interactableOnlyOnce;

    public void Interact()
    {
        Debug.Log(gameObject + " did something.");

        if (toActivateOnInteract != null) { 
            foreach (GameObject obj in toActivateOnInteract) {
                obj.SetActive(true);
            }
        }

        if (toDeactivateOnInteract != null)
        {
            foreach (GameObject obj in toDeactivateOnInteract)
            {
                obj.SetActive(false);
            }
        }

        if (interactableOnlyOnce) GetComponent<BoxCollider2D>().enabled = false;
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

            if (toActivateOnValidInteract != null)
            {
                foreach (GameObject obj in toActivateOnValidInteract)
                {
                    obj.SetActive(true);
                }
            }

            if (toDeactivateOnValidInteract != null)
            {
                foreach (GameObject obj in toDeactivateOnValidInteract)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
