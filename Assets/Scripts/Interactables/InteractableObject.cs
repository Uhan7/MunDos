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
    [ShowIf("checksConditionalObject")] [SerializeField] private bool checkOnValidInteractOnly;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [Header("Item Interactions")]
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    [Header("Flags")]
    [ShowIf("checksConditionalObject")] [HideInInspector] private bool alreadyChecked = false;

    public void Interact()
    {
        SetAll(toActivateOnInteract, true);
        SetAll(toDeactivateOnInteract, false);

        if (conditionalObjectsToCheck != null && !checkOnValidInteractOnly) AddCheck();

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
            if (conditionalObjectsToCheck != null && checkOnValidInteractOnly) AddCheck();
        }
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
    }

    void AddCheck()
    {
        if (alreadyChecked) return;

        foreach (GameObject conditionalObject in conditionalObjectsToCheck)
        {
            ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

            conditionalObjectScript.currentChecks++;
            if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
        }

        alreadyChecked = true;
    }
}
