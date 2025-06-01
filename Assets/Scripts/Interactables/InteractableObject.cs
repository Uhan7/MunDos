using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [SerializeField] private bool itemInteractable;

    public void Interact()
    {
        Debug.Log(gameObject + " did something.");
    }

    public void ItemInteract(bool var)
    {
        if (!itemInteractable) return;

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
