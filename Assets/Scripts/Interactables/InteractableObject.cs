using UnityEngine;
using NaughtyAttributes;

public class InteractableObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] public bool itemInteractable; // Used in PlayerInteract.cs
    [SerializeField] public bool checksConditionalObject;
    [SerializeField] private bool interactableOnlyOnce;

    [Header("Conditionals")]
    [ShowIf("checksConditionalObject")] [SerializeField] private GameObject[] conditionalObjectsToCheck;
    [ShowIf("checksConditionalObject")] [SerializeField] private bool checkOnInteract;
    [ShowIf("checksConditionalObject")] [SerializeField] private bool checkOnValidInteract;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    public void Interact()
    {
        Debug.Log(gameObject + " did something.");

        SetAll(toActivateOnInteract, true);
        SetAll(toDeactivateOnInteract, false);

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
            SetAll(toActivateOnInvalidInteract, true);
            SetAll(toDeactivateOnInvalidInteract, false);
        }
        else
        {
            SetAll(toActivateOnValidInteract, true);
            SetAll(toDeactivateOnValidInteract, false);
        }
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
    }
}
