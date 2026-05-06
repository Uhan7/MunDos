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
        AutoMove autoMoveZone = GetComponent<AutoMove>();
        ObjectsManager objectsManager = GetComponent<ObjectsManager>();
        ZoomEnviManager zoomEnviManager = GetComponent<ZoomEnviManager>();
        DialogueManager dialogueManager = GetComponent<DialogueManager>();
        GameManager gameManager = GetComponent<GameManager>();

        if (interactable != null)
        {
            var comp = new InteractableSaveData();
            comp.alreadyCheckedConditional = interactable.alreadyCheckedConditional;
            comp.alreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
            comp.alreadyCheckedLock = interactable.alreadyCheckedLock;
            data.components.Add(comp);
        }
        if (col != null)
        {
            var comp = new ColliderSaveData();
            comp.colliderState = col.enabled;

            data.components.Add(comp);
        }
        if (conditional != null)
        {
            var comp = new ConditionalSaveData();
            comp.conditionalObjectChecks = conditional.currentChecks;
            data.components.Add(comp);
        }
        if (lockable != null)
        {
            var comp = new LockableSaveData();
            comp.isLockedAtStart = lockable.isLockedAtStart;
            comp.isAlreadyLocked = lockable.isAlreadyLocked;
            comp.lockableObjectChecks = lockable.currentChecks;
            comp.hasStoredLockValue = lockable.hasStoredLockValue;
            comp.storedLockValue = lockable.storedLockValue;
            data.components.Add(comp);
        }
        if (dialogueTrigger != null)
        {
            var comp = new DialogueTriggerSaveData();
            comp.dialogueIsTriggered = dialogueTrigger.dialogueIsTriggered;
            comp.dialogueAlreadyActivatedObjects = dialogueTrigger.alreadyActivatedObjects;
            comp.dialogueAlreadyCheckedConditional = dialogueTrigger.alreadyCheckedConditional;
            comp.dialogueAlreadyDeactivatedObjects = dialogueTrigger.alreadyDeactivatedObjects;
            comp.dialogueAlreadyRemovedPlayeritems = dialogueTrigger.alreadyRemovedPlayeritems;
            comp.dialogueAlreadyGavePlayeritems = dialogueTrigger.alreadyGavePlayeritems;
            data.components.Add(comp);
        }
        if (autoMoveZone != null)
        {
            var comp = new AutoMoveZoneSaveData();
            comp.autoMoveZoneDirection = autoMoveZone.moveDirection;
            data.components.Add(comp);
        }
        if (objectsManager != null)
        {
            var comp = new ObjectsManagerSaveData();
            comp.objectsManagerAlreadyEnabled = objectsManager.alreadyEnabled;
            comp.objectsManagerAlreadyCheckedLock = objectsManager.alreadyCheckedLock;
            comp.objectsManagerAlreadyCheckedUnlock = objectsManager.alreadyCheckedUnlock;
            data.components.Add(comp);
        }
        if (zoomEnviManager != null)
        {
            var comp = new ZoomEnviManagerSaveData();
            comp.zoomEnviManagerActivated = zoomEnviManager.activated;
            data.components.Add(comp);
        }
        if (dialogueManager != null)
        {
            var comp = new DialogueManagerSaveData();
            comp.dialogueManagerWasClicked = dialogueManager.wasClicked;
            comp.dialogueManagerOpen = dialogueManager.open;
            comp.dialogueManagerSkip = dialogueManager.skip;
            comp.dialogueManagerCanNext = dialogueManager.canNext;
            //comp.dialogueManagerCanClick = dialogueManager.canClick;
            comp.dialogueManagerMainCharacterIsSpeaking = dialogueManager.mainCharacterIsSpeaking;
            data.components.Add(comp);
        }
        if (gameManager != null)
        {
            var comp = new GameManagerSaveData();
            comp.gameManagerCanPause = gameManager.canPause;
            comp.gameManagerIsFocusing = gameManager.isFocusing;
            comp.gameManagerIsHidingUI = gameManager.isHidingUI;
            comp.gameManagerIsPaused = gameManager.isPaused;
            data.components.Add(comp);
        }

        if (savePosition) data.position = transform.position;
        if (saveActiveState) data.isActive = gameObject.activeSelf;

        return data;
    }

    public void RestoreState(ObjectSaveData data)
    {
        foreach (var comp in data.components)
        {
            if (comp.type == "Interactable")
            {
                var interactable = GetComponent<InteractableObject>();
                var compData = comp as InteractableSaveData;

                if (interactable != null && compData != null)
                {
                    interactable.alreadyCheckedConditional = compData.alreadyCheckedConditional;
                    interactable.alreadyCheckedUnlock = compData.alreadyCheckedUnlock;
                    interactable.alreadyCheckedLock = compData.alreadyCheckedLock;
                }
            }

            if (comp.type == "Collider")
            {
                var col = GetComponent<Collider2D>();
                var compData = comp as ColliderSaveData;

                if (col != null && compData != null)
                {
                    col.enabled = compData.colliderState;
                }
            }

            if (comp.type == "Conditional")
            {
                var conditional = GetComponent<ConditionalObject>();
                var compData = comp as ConditionalSaveData;

                if (conditional != null && compData != null)
                {
                    conditional.currentChecks = compData.conditionalObjectChecks;
                }
            }

            if (comp.type == "Lockable")
            {
                var lockable = GetComponent<LockableObject>();
                var compData = comp as LockableSaveData;

                if (lockable != null && compData != null)
                {
                    lockable.isLockedAtStart = compData.isLockedAtStart;
                    lockable.isAlreadyLocked = compData.isAlreadyLocked;
                    lockable.currentChecks = compData.lockableObjectChecks;
                    lockable.hasStoredLockValue = compData.hasStoredLockValue;
                    lockable.storedLockValue = compData.storedLockValue;
                }
            }

            if (comp.type == "DialogueTrigger")
            {
                var dialogueTrigger = GetComponent<DialogueTrigger>();
                var compData = comp as DialogueTriggerSaveData;

                if (dialogueTrigger != null && compData != null)
                {
                    dialogueTrigger.dialogueIsTriggered = compData.dialogueIsTriggered;
                    dialogueTrigger.alreadyActivatedObjects = compData.dialogueAlreadyActivatedObjects;
                    dialogueTrigger.alreadyCheckedConditional = compData.dialogueAlreadyCheckedConditional;
                    dialogueTrigger.alreadyDeactivatedObjects = compData.dialogueAlreadyDeactivatedObjects;
                    dialogueTrigger.alreadyRemovedPlayeritems = compData.dialogueAlreadyRemovedPlayeritems;
                    dialogueTrigger.alreadyGavePlayeritems = compData.dialogueAlreadyGavePlayeritems;
                }
            }

            if (comp.type == "AutoMoveZone")
            {
                var autoMove = GetComponent<AutoMove>();
                var compData = comp as AutoMoveZoneSaveData;

                if (autoMove != null && compData != null)
                {
                    autoMove.moveDirection = compData.autoMoveZoneDirection;
                }
            }

            if (comp.type == "ObjectsManager")
            {
                var objectsManager = GetComponent<ObjectsManager>();
                var compData = comp as ObjectsManagerSaveData;

                if (objectsManager != null && compData != null)
                {
                    objectsManager.alreadyEnabled = compData.objectsManagerAlreadyEnabled;
                    objectsManager.alreadyCheckedLock = compData.objectsManagerAlreadyCheckedLock;
                    objectsManager.alreadyCheckedUnlock = compData.objectsManagerAlreadyCheckedUnlock;
                }
            }

            if (comp.type == "ZoomEnviManager")
            {
                var zoom = GetComponent<ZoomEnviManager>();
                var compData = comp as ZoomEnviManagerSaveData;

                if (zoom != null && compData != null)
                {
                    zoom.activated = compData.zoomEnviManagerActivated;
                }
            }

            if (comp.type == "DialogueManager")
            {
                var dialogueManager = GetComponent<DialogueManager>();
                var compData = comp as DialogueManagerSaveData;

                if (dialogueManager != null && compData != null)
                {
                    dialogueManager.wasClicked = compData.dialogueManagerWasClicked;
                    dialogueManager.open = compData.dialogueManagerOpen;
                    dialogueManager.skip = compData.dialogueManagerSkip;
                    dialogueManager.canNext = compData.dialogueManagerCanNext;
                    //dialogueManager.canClick = compData.dialogueManagerCanClick;
                    dialogueManager.mainCharacterIsSpeaking = compData.dialogueManagerMainCharacterIsSpeaking;
                }
            }

            if (comp.type == "GameManager")
            {
                var gameManager = GetComponent<GameManager>();
                var compData = comp as GameManagerSaveData;

                if (gameManager != null && compData != null)
                {
                    gameManager.canPause = compData.gameManagerCanPause;
                    gameManager.isFocusing = compData.gameManagerIsFocusing;
                    gameManager.isHidingUI = compData.gameManagerIsHidingUI;
                    gameManager.isPaused = compData.gameManagerIsPaused;
                }
            }
        }

        if (savePosition) transform.position = data.position;
        if (saveActiveState) gameObject.SetActive(data.isActive);
    }
}
