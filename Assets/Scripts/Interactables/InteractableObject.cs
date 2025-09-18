using UnityEngine;
using NaughtyAttributes;

public class InteractableObject : MonoBehaviour
{

    [Header("Feedback")]
    [HideInInspector] public SpriteRenderer spriteRenderer; // Used in PlayerInteract.cs
    [HideInInspector] public Sprite normalSprite; // Used in PlayerInteract.cs
    [SerializeField] public Sprite outlinedSprite; // Used in PlayerInteract.cs

    [Header("Properties")]
    [SerializeField] public bool itemInteractable; // Used in PlayerInteract.cs
    [SerializeField] private bool checksConditionalObject;
    [SerializeField] private bool unlockInteractableObject;
    [SerializeField] private bool lockInteractableObject;

    [Header("Conditionals")]
    [ShowIf("checksConditionalObject")] [SerializeField] private GameObject[] conditionalObjectsToCheck;
    [ShowIf("checksConditionalObject")] [SerializeField] private bool checkOnValidInteractOnly;

    [Header("Locked Interactions")]
    [ShowIf("unlockInteractableObject")] [SerializeField] private GameObject[] objectsToUnlockCheck;
    [ShowIf("lockInteractableObject")] [SerializeField] private GameObject[] objectsToLockCheck;
    [ShowIf("unlockInteractableObject")] [SerializeField] private bool unlockOnValidInteractOnly;
    [ShowIf("lockInteractableObject")] [SerializeField] private bool lockOnValidInteractOnly;

    [Header("Interactions")]
    [SerializeField] private GameObject[] toActivateOnInteract;
    [SerializeField] private GameObject[] toDeactivateOnInteract;

    [Header("Item Interactions")]
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnValidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toActivateOnInvalidInteract;
    [ShowIf("itemInteractable")] [SerializeField] private GameObject[] toDeactivateOnInvalidInteract;

    [Header("Flags")]
    [ShowIf("checksConditionalObject")] [HideInInspector] private bool alreadyCheckedConditional = false;
    [ShowIf("unlockInteractableObject")] [HideInInspector] private bool alreadyCheckedUnlock = false;
    [ShowIf("lockInteractableObject")] [HideInInspector] private bool alreadyCheckedLock = false;

    private void Awake()
    {
        InitializeCache();
    }

    public void Interact()
    {
        SetAll(toActivateOnInteract, true);
        SetAll(toDeactivateOnInteract, false);

        if (conditionalObjectsToCheck != null && !checkOnValidInteractOnly) AddConditionalCheck();
        if (objectsToUnlockCheck != null && !unlockOnValidInteractOnly) AddUnlockCheck();
        if (objectsToLockCheck != null && !lockOnValidInteractOnly) AddLockCheck();

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

            if (conditionalObjectsToCheck != null && checkOnValidInteractOnly) AddConditionalCheck();
            if (objectsToUnlockCheck != null && unlockOnValidInteractOnly) AddUnlockCheck();
            if (objectsToLockCheck != null && lockOnValidInteractOnly) AddLockCheck();
        }
    }

    void SetAll(GameObject[] objects, bool value)
    {
        if (objects == null) return;
        foreach (GameObject obj in objects) obj.SetActive(value);
    }

    void AddConditionalCheck()
    {
        if (alreadyCheckedConditional) return;

        foreach (GameObject conditionalObject in conditionalObjectsToCheck)
        {
            ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

            conditionalObjectScript.currentChecks++;
            if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
        }

        alreadyCheckedConditional = true;
    }

    void AddUnlockCheck()
    {
        if (alreadyCheckedUnlock) return;

        foreach (GameObject lockObject in objectsToUnlockCheck)
        {
            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();

            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks) lockObjectScript.Lock(false);
        }

        alreadyCheckedUnlock = true;
    }

    void AddLockCheck()
    {
        if (alreadyCheckedLock) return;

        foreach (GameObject lockObject in objectsToLockCheck)
        {
            LockableObject lockObjectScript = lockObject.GetComponent<LockableObject>();

            lockObjectScript.currentChecks++;
            if (lockObjectScript.currentChecks >= lockObjectScript.requiredChecks) lockObjectScript.Lock(true);
        }

        alreadyCheckedLock = true;
    }

    // Helper Functions --------------------------------------------------------

    private void InitializeCache()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = spriteRenderer.sprite;

        if (outlinedSprite == null) outlinedSprite = normalSprite;
    }
}
