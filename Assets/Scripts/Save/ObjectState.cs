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
        Collider2D col = GetComponent<Collider2D>();

        if (interactable != null || lockable != null) saveColliderState = true;
        if (col != null) saveColliderState = true;

        if (savePosition) data.position = transform.position;
        if (conditional != null) data.conditionalObjectChecks = conditional.currentChecks;
        if (interactable != null)
        {
            data.interactableAlreadyCheckedConditionals = interactable.alreadyCheckedConditional;
            data.interactableAlreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
            data.interactableAlreadyCheckedLock = interactable.alreadyCheckedLock;
        }
        if (saveColliderState) data.colliderState = GetComponent<Collider2D>().enabled;
        if (lockable != null)
        {
            data.isLockedAtStart = lockable.isLockedAtStart;
            data.isAlreadyLocked = lockable.isAlreadyLocked;
            data.lockableObjectChecks = lockable.currentChecks;
            data.hasStoredLockValue = lockable.hasStoredLockValue;
            data.storedLockValue = lockable.storedLockValue;
        }

        if (saveActiveState) data.isActive = gameObject.activeSelf;

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
        if (interactable != null)
        {
            interactable.alreadyCheckedConditional = data.interactableAlreadyCheckedConditionals;
            interactable.alreadyCheckedUnlock = data.interactableAlreadyCheckedUnlock;
            interactable.alreadyCheckedLock = data.interactableAlreadyCheckedLock;
        }
        if (saveColliderState) GetComponent<Collider2D>().enabled = data.colliderState;
        if (lockable != null)
        {
            lockable.isLockedAtStart = data.isLockedAtStart;
            lockable.isAlreadyLocked = data.isAlreadyLocked;
            lockable.currentChecks = data.lockableObjectChecks;
            lockable.hasStoredLockValue = data.hasStoredLockValue;
            lockable.storedLockValue = data.storedLockValue;
        }

        if (saveActiveState) gameObject.SetActive(data.isActive);
    }
}
