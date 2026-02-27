using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    // Variables ---------------------------------------------------------------
    [Header("Variables To Save")]
    public bool saveActiveState = true;
    public bool savePosition = false;
    public bool saveColliderState = false;

    // [Header("Required Variables References")]
    // private ConditionalObject conditional;
    // private LockableObject lockable;
    // private InteractableObject interactable;

    // Functions ---------------------------------------------------------------
    private void Awake()
    {
        ConditionalObject conditional = GetComponent<ConditionalObject>();
        LockableObject lockable = GetComponent<LockableObject>();
        InteractableObject interactable = GetComponent<InteractableObject>();
        Collider2D col = GetComponent<Collider2D>();

        if (interactable != null || lockable != null) saveColliderState = true;
        if (col != null) saveColliderState = true;
    }

    public ObjectSaveData CaptureState()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        ConditionalObject conditional = GetComponent<ConditionalObject>();
        LockableObject lockable = GetComponent<LockableObject>();
        InteractableObject interactable = GetComponent<InteractableObject>();

        if (interactable != null || lockable != null) saveColliderState = true;

        if (savePosition) data.position = transform.position;
        if (conditional != null) data.conditionalObjectChecks = conditional.currentChecks;
        if (lockable != null)
        {
            data.isLockedAtStart = lockable.isLockedAtStart;
            data.isAlreadyLocked = lockable.isAlreadyLocked;
            data.lockableObjectChecks = lockable.currentChecks;
            data.hasStoredLockValue = lockable.hasStoredLockValue;
            data.storedLockValue = lockable.storedLockValue;
        }
        if (interactable != null)
        {
            data.interactableAlreadyCheckedConditionals = interactable.alreadyCheckedConditional;
            data.interactableAlreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
            data.interactableAlreadyCheckedLock = interactable.alreadyCheckedLock;
        }
        if (saveActiveState) data.isActive = gameObject.activeSelf;
        if (saveColliderState) data.colliderState = GetComponent<Collider2D>().enabled;

        if (gameObject.name == "L1-DZ9") Debug.Log($"Saving {name} | Checks: {conditional?.currentChecks}");

        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        ConditionalObject conditional = GetComponent<ConditionalObject>();
        LockableObject lockable = GetComponent<LockableObject>();
        InteractableObject interactable = GetComponent<InteractableObject>();
        Collider2D col = GetComponent<Collider2D>();

        if (interactable != null || lockable != null) saveColliderState = true;
        if (col != null) saveColliderState = true;

        if (savePosition) transform.position = data.position;
        if (conditional != null) conditional.currentChecks = data.conditionalObjectChecks;
        if (lockable != null)
        {
            lockable.isLockedAtStart = data.isLockedAtStart;
            lockable.isAlreadyLocked = data.isAlreadyLocked;
            lockable.currentChecks = data.lockableObjectChecks;
            lockable.hasStoredLockValue = data.hasStoredLockValue;
            lockable.storedLockValue = data.storedLockValue;
        }
        if (interactable != null)
        {
            interactable.alreadyCheckedConditional = data.interactableAlreadyCheckedConditionals;
            interactable.alreadyCheckedUnlock = data.interactableAlreadyCheckedUnlock;
            interactable.alreadyCheckedLock = data.interactableAlreadyCheckedLock;
        }
        if (saveActiveState) gameObject.SetActive(data.isActive);
        if (saveColliderState) GetComponent<Collider2D>().enabled = data.colliderState;

        if (gameObject.name == "L1-DZ9") Debug.Log($"Loading {name} | Checks: {data.conditionalObjectChecks}");
    }
}
