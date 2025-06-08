using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public bool itemInteractable; // Used in PlayerInteract.cs

    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [SerializeField] private GameObject[] toActivateOnCorrectInteract;
    [SerializeField] private GameObject[] toDeactivateOnCorrectInteract;

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

            if (toActivateOnCorrectInteract != null)
            {
                foreach (GameObject obj in toActivateOnCorrectInteract)
                {
                    obj.SetActive(true);
                }
            }

            if (toDeactivateOnCorrectInteract != null)
            {
                foreach (GameObject obj in toDeactivateOnCorrectInteract)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
