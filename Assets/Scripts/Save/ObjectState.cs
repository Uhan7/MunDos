using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    // Variables ---------------------------------------------------------------
    [Header("Variables To Save")]
    public bool saveActiveState = true;
    public bool savePosition = false;
    public bool saveColliderState = false;

    [Header("Required Variables References")]
    private ConditionalObject conditional;
    private LockableObject lockable;
    private InteractableObject interactable;

    // Functions ---------------------------------------------------------------
    private void Awake()
    {
        conditional = GetComponent<ConditionalObject>();
        lockable = GetComponent<LockableObject>();
        interactable = GetComponent<InteractableObject>();

        if (conditional || lockable) saveColliderState = true;
    }

    public ObjectSaveData CaptureState()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        if (saveActiveState) data.isActive = gameObject.activeSelf;
        if (savePosition) data.position = transform.position;
        if (saveColliderState) data.colliderState = GetComponent<Collider2D>().enabled;
        if (conditional != null) data.conditionalObjectChecks = conditional.currentChecks;
        if (lockable != null) data.lockableObjectChecks = lockable.currentChecks;
        if (interactable != null)
        {
            data.interactableAlreadyCheckedConditionals = interactable.alreadyCheckedConditional;
            data.interactableAlreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
            data.interactableAlreadyCheckedLock = interactable.alreadyCheckedLock;
        }

        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        if (saveActiveState) gameObject.SetActive(data.isActive);
        if (savePosition) transform.position = data.position;
        if (saveColliderState) GetComponent<Collider2D>().enabled = data.colliderState;
        if (conditional != null) conditional.currentChecks = data.conditionalObjectChecks;
        if (lockable != null) lockable.currentChecks = data.lockableObjectChecks;
        if (interactable != null)
        {
            interactable.alreadyCheckedConditional = data.interactableAlreadyCheckedConditionals;
            interactable.alreadyCheckedUnlock = data.interactableAlreadyCheckedUnlock;
            interactable.alreadyCheckedLock = data.interactableAlreadyCheckedLock;
        }
    }
}
