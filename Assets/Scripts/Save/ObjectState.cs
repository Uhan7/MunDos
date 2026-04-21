using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    // Variables ---------------------------------------------------------------
    [Header("Variables To Save")]
    public bool saveActiveState = true;
    public bool savePosition = false; // Probably only to NPCs and Protags

    // Functions ---------------------------------------------------------------
    public ObjectSaveData CaptureState()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        InteractableObject interactable = GetComponent<InteractableObject>();
        Collider2D col = GetComponent<Collider2D>();
        ConditionalObject conditional = GetComponent<ConditionalObject>();
        LockableObject lockable = GetComponent<LockableObject>();
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        ObjectsManager objectsManager = GetComponent<ObjectsManager>();
        GameManager gameManager = GetComponent<GameManager>();
        ZoomEnviManager zoomEnviManager = GetComponent<ZoomEnviManager>();

        if (interactable != null)
        {
            data.interactableAlreadyCheckedConditionals = interactable.alreadyCheckedConditional;
            data.interactableAlreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
            data.interactableAlreadyCheckedLock = interactable.alreadyCheckedLock;
        }
        if (col != null)
        {
            data.colliderState = col.enabled;
        }
        if (conditional != null)
        {
            data.conditionalObjectChecks = conditional.currentChecks;
        }
        if (lockable != null)
        {
            data.isLockedAtStart = lockable.isLockedAtStart;
            data.isAlreadyLocked = lockable.isAlreadyLocked;
            data.lockableObjectChecks = lockable.currentChecks;
            data.hasStoredLockValue = lockable.hasStoredLockValue;
            data.storedLockValue = lockable.storedLockValue;
        }
        if (dialogueTrigger != null)
        {
            data.dialogueIsTriggered = dialogueTrigger.dialogueIsTriggered;
            data.dialogueAlreadyActivatedObjects = dialogueTrigger.alreadyActivatedObjects;
            data.dialogueAlreadyCheckedConditional = dialogueTrigger.alreadyCheckedConditional;
            data.dialogueAlreadyDeactivatedObjects = dialogueTrigger.alreadyDeactivatedObjects;
            data.dialogueAlreadyRemovedPlayeritems = dialogueTrigger.alreadyRemovedPlayeritems;
        }
        if (objectsManager != null)
        {
            data.objectManagerAlreadyEnabled = objectsManager.alreadyEnabled;
            data.objectManagerAlreadyCheckedLock = objectsManager.alreadyCheckedLock;
            data.objectManagerAlreadyCheckedUnlock = objectsManager.alreadyCheckedUnlock;
        }
        if (zoomEnviManager != null)
        {
            data.zoomEnviManagerActivated = zoomEnviManager.activated;
        }
        if (gameManager != null)
        {
            data.gameManagerCanPause = gameManager.canPause;
            data.gameManagerIsFocusing = gameManager.isFocusing;
            data.gameManagerIsHidingUI = gameManager.isHidingUI;
            data.gameManagerIsPaused = gameManager.isPaused;
        }

        if (savePosition) data.position = transform.position;
        if (saveActiveState) data.isActive = gameObject.activeSelf;

        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        InteractableObject interactable = GetComponent<InteractableObject>();
        Collider2D col = GetComponent<Collider2D>();
        ConditionalObject conditional = GetComponent<ConditionalObject>();
        LockableObject lockable = GetComponent<LockableObject>();
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        ObjectsManager objectsManager = GetComponent<ObjectsManager>();
        ZoomEnviManager zoomEnviManager = GetComponent<ZoomEnviManager>();
        GameManager gameManager = GetComponent<GameManager>();

        if (interactable != null)
        {
            interactable.alreadyCheckedConditional = data.interactableAlreadyCheckedConditionals;
            interactable.alreadyCheckedUnlock = data.interactableAlreadyCheckedUnlock;
            interactable.alreadyCheckedLock = data.interactableAlreadyCheckedLock;
        }
        if (col != null)
        {
            col.enabled = data.colliderState;
        }
        if (conditional != null)
        {
            conditional.currentChecks = data.conditionalObjectChecks;
        }
        if (lockable != null)
        {
            lockable.isLockedAtStart = data.isLockedAtStart;
            lockable.isAlreadyLocked = data.isAlreadyLocked;
            lockable.currentChecks = data.lockableObjectChecks;
            lockable.hasStoredLockValue = data.hasStoredLockValue;
            lockable.storedLockValue = data.storedLockValue;
        }
        if (dialogueTrigger != null)
        {
            dialogueTrigger.dialogueIsTriggered = data.dialogueIsTriggered;
            dialogueTrigger.alreadyActivatedObjects = data.dialogueAlreadyActivatedObjects;
            dialogueTrigger.alreadyCheckedConditional = data.dialogueAlreadyCheckedConditional;
            dialogueTrigger.alreadyDeactivatedObjects = data.dialogueAlreadyDeactivatedObjects;
            dialogueTrigger.alreadyRemovedPlayeritems = data.dialogueAlreadyRemovedPlayeritems;
        }
        if (objectsManager != null)
        {
            objectsManager.alreadyEnabled = data.objectManagerAlreadyEnabled;
            objectsManager.alreadyCheckedLock = data.objectManagerAlreadyCheckedLock;
            objectsManager.alreadyCheckedUnlock = data.objectManagerAlreadyCheckedUnlock;
        }
        if (zoomEnviManager != null)
        {
            zoomEnviManager.activated = data.zoomEnviManagerActivated;
        }
        if (gameManager != null)
        {
            gameManager.canPause = data.gameManagerCanPause;
            gameManager.isFocusing = data.gameManagerIsFocusing;
            gameManager.isHidingUI = data.gameManagerIsHidingUI;
            gameManager.isPaused = data.gameManagerIsPaused;
        }

        if (savePosition) transform.position = data.position;
        if (saveActiveState) gameObject.SetActive(data.isActive);
    }
}
