using UnityEngine;
using System.Collections.Generic;

// ===============
// For every new shit to add... read the TODOs
// ===============

[RequireComponent(typeof(UniqueID))]
public class ObjectState : MonoBehaviour
{
    // Variables ---------------------------------------------------------------
    [Header("Variables To Save")]
    [SerializeField] public bool saveActiveState = true;
    [SerializeField] public bool savePosition = false; // Probably only to NPCs and Protags

    [HideInInspector] private bool defaultsInitialized;
    [HideInInspector] private bool defaultIsActive;
    [HideInInspector] private Vector3 defaultPosition;

    // TODO 1: add the default savedata here (make sure u alrdy made savedata of the component separately)
    private InteractableSaveData defaultInteractableData;
    // -- multi interactable goes here --
    private OilSaveData defaultOilData;
    private ColliderSaveData defaultColliderData;
    private ConditionalSaveData defaultConditionalData;
    private LockableSaveData defaultLockableData;
    private DialogueTriggerSaveData defaultDialogueTriggerData;
    private AutoMoveZoneSaveData defaultAutoMoveData;
    private ObjectsManagerSaveData defaultObjectsManagerData;
    private ZoomEnviManagerSaveData defaultZoomEnviManagerData;
    private DialogueManagerSaveData defaultDialogueManagerData;
    private DialogueLogSaveData defaultDialogueLogData;
    private QuestLogSaveData defaultQuestLogData;
    private ProgressionPointSaveData defaultProgressionPointData;
    private GameManagerSaveData defaultGameManagerData;

    // Functions ---------------------------------------------------------------
    public void InitializeDefaultState()
    {
        if (defaultsInitialized) return;

        defaultIsActive = gameObject.activeSelf;
        defaultPosition = transform.position;

        // TODO 2: capture the default data (it'll say doesnt exist, we will fill it later)
        // "Save" at the start to set the default shihs
        defaultInteractableData = CaptureInteractableData();
        // -- multi interactable goes here --
        defaultOilData = CaptureOilData();
        defaultColliderData = CaptureColliderData();
        defaultConditionalData = CaptureConditionalData();
        defaultLockableData = CaptureLockableData();
        defaultDialogueTriggerData = CaptureDialogueTriggerData();
        defaultAutoMoveData = CaptureAutoMoveData();
        defaultObjectsManagerData = CaptureObjectsManagerData();
        defaultZoomEnviManagerData = CaptureZoomEnviManagerData();
        defaultDialogueManagerData = CaptureDialogueManagerData();
        defaultDialogueLogData = CaptureDialogueLogData();
        defaultQuestLogData = CaptureQuestLogData();
        defaultProgressionPointData = CaptureProgressionPointData();
        defaultGameManagerData = CaptureGameManagerData();

        defaultsInitialized = true;
    }

    public ObjectSaveData CaptureState()
    {
        InitializeDefaultState();

        ObjectSaveData data = new ObjectSaveData();
        data.objectID = GetComponent<UniqueID>().ID;

        if (saveActiveState && gameObject.activeSelf != defaultIsActive)
        {
            data.changedActive = true;
            data.isActive = gameObject.activeSelf;
        }

        if (savePosition && transform.position != defaultPosition)
        {
            data.changedPosition = true;
            data.position = transform.position;
        }

        // TODO 3: Add the component delta to the new component (again it'll say not defined yet)
        AddComponentDelta(data.components, CaptureInteractableData(), defaultInteractableData);
        // -- multi interactable goes here --
        AddComponentDelta(data.components, CaptureOilData(), defaultOilData);
        AddComponentDelta(data.components, CaptureColliderData(), defaultColliderData);
        AddComponentDelta(data.components, CaptureConditionalData(), defaultConditionalData);
        AddComponentDelta(data.components, CaptureLockableData(), defaultLockableData);
        AddComponentDelta(data.components, CaptureDialogueTriggerData(), defaultDialogueTriggerData);
        AddComponentDelta(data.components, CaptureAutoMoveData(), defaultAutoMoveData);
        AddComponentDelta(data.components, CaptureObjectsManagerData(), defaultObjectsManagerData);
        AddComponentDelta(data.components, CaptureZoomEnviManagerData(), defaultZoomEnviManagerData);
        AddComponentDelta(data.components, CaptureDialogueManagerData(), defaultDialogueManagerData);
        AddComponentDelta(data.components, CaptureDialogueLogData(), defaultDialogueLogData);
        AddComponentDelta(data.components, CaptureQuestLogData(), defaultQuestLogData);
        AddComponentDelta(data.components, CaptureProgressionPointData(), defaultProgressionPointData);
        AddComponentDelta(data.components, CaptureGameManagerData(), defaultGameManagerData);

        bool hasAnyDelta = data.changedActive || data.changedPosition || data.components.Count > 0;

        if (hasAnyDelta) return data;
        else return null;
    }

        private void AddComponentDelta(List<ComponentSaveData> list, ComponentSaveData current, ComponentSaveData baseline)
    {
        if (current == null) return;
        if (baseline == null)
        {
            list.Add(current);
            return;
        }

        if (!ComponentDataEquals(current, baseline))
        {
            list.Add(current);
        }
    }

    private bool ComponentDataEquals(ComponentSaveData A, ComponentSaveData B)
    {
        // This part basically checks, did anything change from default?
        // If yes, then consider that in the save json, if not, then IDGAF

        // TODO 4: Now to the logic for comparing the values between the saved and the actual

        if (A == null || B == null) return A == B;
        if (A.type != B.type) return false;

        if (A is InteractableSaveData interactableA && B is InteractableSaveData interactableB)
        {
            return interactableA.alreadyCheckedConditional == interactableB.alreadyCheckedConditional
                && interactableA.alreadyCheckedUnlock == interactableB.alreadyCheckedUnlock
                && interactableA.alreadyCheckedLock == interactableB.alreadyCheckedLock;
        }

        // -- multi interactable goes here --

        if (A is OilSaveData oilA && B is OilSaveData oilB)
        {
            return oilA.pH == oilB.pH
                && oilA.salinity== oilB.salinity
                && oilA.finished == oilB.finished;
        }

        if (A is ColliderSaveData colliderA && B is ColliderSaveData colliderB)
        {
            return colliderA.colliderState == colliderB.colliderState;
        }

        if (A is ConditionalSaveData conditionalA && B is ConditionalSaveData conditionalB)
        {
            return conditionalA.conditionalObjectChecks == conditionalB.conditionalObjectChecks;
        }

        if (A is LockableSaveData lockableA && B is LockableSaveData lockableB)
        {
            return lockableA.isLockedAtStart == lockableB.isLockedAtStart
                && lockableA.isAlreadyLocked == lockableB.isAlreadyLocked
                && lockableA.lockableObjectChecks == lockableB.lockableObjectChecks
                && lockableA.hasStoredLockValue == lockableB.hasStoredLockValue
                && lockableA.storedLockValue == lockableB.storedLockValue;
        }

        if (A is DialogueTriggerSaveData dialogueTriggerA && B is DialogueTriggerSaveData dialogueTriggerB)
        {
            return
                //dialogueTriggerA.dialogueIsTriggered == dialogueTriggerB.dialogueIsTriggered &&
                dialogueTriggerA.dialogueAlreadyActivatedObjects == dialogueTriggerB.dialogueAlreadyActivatedObjects
                && dialogueTriggerA.dialogueAlreadyCheckedConditional == dialogueTriggerB.dialogueAlreadyCheckedConditional
                && dialogueTriggerA.dialogueAlreadyDeactivatedObjects == dialogueTriggerB.dialogueAlreadyDeactivatedObjects
                && dialogueTriggerA.dialogueAlreadyRemovedPlayeritems == dialogueTriggerB.dialogueAlreadyRemovedPlayeritems
                && dialogueTriggerA.dialogueAlreadyGavePlayeritems == dialogueTriggerB.dialogueAlreadyGavePlayeritems;
        }

        if (A is AutoMoveZoneSaveData autoMoveA && B is AutoMoveZoneSaveData autoMoveB)
        {
            return autoMoveA.autoMoveZoneDirection == autoMoveB.autoMoveZoneDirection;
        }

        if (A is ObjectsManagerSaveData objectsManagerA && B is ObjectsManagerSaveData objectsManagerB)
        {
            return objectsManagerA.objectsManagerAlreadyEnabled == objectsManagerB.objectsManagerAlreadyEnabled
                && objectsManagerA.objectsManagerAlreadyCheckedUnlock == objectsManagerB.objectsManagerAlreadyCheckedUnlock
                && objectsManagerA.objectsManagerAlreadyCheckedLock == objectsManagerB.objectsManagerAlreadyCheckedLock;
        }

        if (A is ZoomEnviManagerSaveData zoomEnviA && B is ZoomEnviManagerSaveData zoomEnviB)
        {
            return zoomEnviA.zoomEnviManagerActivated == zoomEnviB.zoomEnviManagerActivated;
        }

        if (A is DialogueManagerSaveData dialogueManagerA && B is DialogueManagerSaveData dialogueManagerB)
        {
            return dialogueManagerA.dialogueManagerWasClicked == dialogueManagerB.dialogueManagerWasClicked
                && dialogueManagerA.dialogueManagerOpen == dialogueManagerB.dialogueManagerOpen
                && dialogueManagerA.dialogueManagerSkip == dialogueManagerB.dialogueManagerSkip
                && dialogueManagerA.dialogueManagerCanNext == dialogueManagerB.dialogueManagerCanNext
                && dialogueManagerA.dialogueManagerMainCharacterIsSpeaking == dialogueManagerB.dialogueManagerMainCharacterIsSpeaking;
        }

        if (A is DialogueLogSaveData dialogueLogA && B is DialogueLogSaveData dialogueLogB)
        {
            // A bit diff since this is a loong list of strings
            if (dialogueLogA.storedDialogues.Count != dialogueLogB.storedDialogues.Count) return false;

            for (int i = 0; i < dialogueLogA.storedDialogues.Count; i++) if (dialogueLogA.storedDialogues[i] != dialogueLogB.storedDialogues[i]) return false;

            return true;
        }

        if (A is QuestLogSaveData questLogA && B is QuestLogSaveData questLogB)
        {
            return questLogA.actualQuestTextData == questLogB.actualQuestTextData
                && questLogA.guideTextData == questLogB.guideTextData;
        }

        if (A is ProgressionPointSaveData progressionPointA && B is ProgressionPointSaveData progressionPointB)
        {
            return progressionPointA.hasBeenActivated == progressionPointB.hasBeenActivated;
        }

        if (A is GameManagerSaveData gameManagerA && B is GameManagerSaveData gameManagerB)
        {
            return gameManagerA.gameManagerCanPause == gameManagerB.gameManagerCanPause
                && gameManagerA.gameManagerIsFocusing == gameManagerB.gameManagerIsFocusing
                && gameManagerA.gameManagerIsHidingUI == gameManagerB.gameManagerIsHidingUI
                && gameManagerA.gameManagerIsPaused == gameManagerB.gameManagerIsPaused;
        }

        return false;
    }

    // TODO 5: Thennn create a capture function (You will reference the actual component, then "write" it to data)

    private InteractableSaveData CaptureInteractableData()
    {
        InteractableObject interactable = GetComponent<InteractableObject>();
        if (interactable == null) return null;

        var data = new InteractableSaveData();
        data.alreadyCheckedConditional = interactable.alreadyCheckedConditional;
        data.alreadyCheckedUnlock = interactable.alreadyCheckedUnlock;
        data.alreadyCheckedLock = interactable.alreadyCheckedLock;
        return data;
    }

    // -- multi interactable goes here --

    private OilSaveData CaptureOilData()
    {
        Oil oil = GetComponent<Oil>();
        if (oil == null) return null;

        var data = new OilSaveData();
        data.pH = oil.pH;
        data.salinity = oil.salinity;
        data.finished = oil.finished;
        return data;
    }

    private ColliderSaveData CaptureColliderData()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return null;

        var data = new ColliderSaveData();
        data.colliderState = col.enabled;
        return data;
    }

    private ConditionalSaveData CaptureConditionalData()
    {
        ConditionalObject conditional = GetComponent<ConditionalObject>();
        if (conditional == null) return null;

        var data = new ConditionalSaveData();
        data.conditionalObjectChecks = conditional.currentChecks;
        return data;
    }

    private LockableSaveData CaptureLockableData()
    {
        LockableObject lockable = GetComponent<LockableObject>();
        if (lockable == null) return null;

        var data = new LockableSaveData();
        data.isLockedAtStart = lockable.isLockedAtStart;
        data.isAlreadyLocked = lockable.isAlreadyLocked;
        data.lockableObjectChecks = lockable.currentChecks;
        data.hasStoredLockValue = lockable.hasStoredLockValue;
        data.storedLockValue = lockable.storedLockValue;
        return data;
    }

    private DialogueTriggerSaveData CaptureDialogueTriggerData()
    {
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        if (dialogueTrigger == null) return null;

        var data = new DialogueTriggerSaveData();
        //data.dialogueIsTriggered = dialogueTrigger.dialogueIsTriggered;
        data.dialogueAlreadyActivatedObjects = dialogueTrigger.alreadyActivatedObjects;
        data.dialogueAlreadyCheckedConditional = dialogueTrigger.alreadyCheckedConditional;
        data.dialogueAlreadyDeactivatedObjects = dialogueTrigger.alreadyDeactivatedObjects;
        data.dialogueAlreadyRemovedPlayeritems = dialogueTrigger.alreadyRemovedPlayeritems;
        data.dialogueAlreadyGavePlayeritems = dialogueTrigger.alreadyGavePlayeritems;
        return data;
    }

    private AutoMoveZoneSaveData CaptureAutoMoveData()
    {
        AutoMove autoMoveZone = GetComponent<AutoMove>();
        if (autoMoveZone == null) return null;

        var data = new AutoMoveZoneSaveData();
        data.autoMoveZoneDirection = autoMoveZone.moveDirection;
        return data;
    }

    private ObjectsManagerSaveData CaptureObjectsManagerData()
    {
        ObjectsManager objectsManager = GetComponent<ObjectsManager>();
        if (objectsManager == null) return null;

        var data = new ObjectsManagerSaveData();
        data.objectsManagerAlreadyEnabled = objectsManager.alreadyEnabled;
        data.objectsManagerAlreadyCheckedLock = objectsManager.alreadyCheckedLock;
        data.objectsManagerAlreadyCheckedUnlock = objectsManager.alreadyCheckedUnlock;
        return data;
    }

    private ZoomEnviManagerSaveData CaptureZoomEnviManagerData()
    {
        ZoomEnviManager zoomEnviManager = GetComponent<ZoomEnviManager>();
        if (zoomEnviManager == null) return null;

        var data = new ZoomEnviManagerSaveData();
        data.zoomEnviManagerActivated = zoomEnviManager.activated;
        return data;
    }

    private DialogueManagerSaveData CaptureDialogueManagerData()
    {
        DialogueManager dialogueManager = GetComponent<DialogueManager>();
        if (dialogueManager == null) return null;

        var data = new DialogueManagerSaveData();
        data.dialogueManagerWasClicked = dialogueManager.wasClicked;
        data.dialogueManagerOpen = dialogueManager.open;
        data.dialogueManagerSkip = dialogueManager.skip;
        data.dialogueManagerCanNext = dialogueManager.canNext;
        data.dialogueManagerMainCharacterIsSpeaking = dialogueManager.mainCharacterIsSpeaking;
        return data;
    }

    private DialogueLogSaveData CaptureDialogueLogData()
    {
        LogScreen dialogueLog = GetComponent<LogScreen>();
        if (dialogueLog == null) return null;

        var data = new DialogueLogSaveData();
        data.storedDialogues = dialogueLog.GetStoredDialogues();
        return data;
    }

    private QuestLogSaveData CaptureQuestLogData()
    {
        QuestLogManager questLogManager = GetComponent<QuestLogManager>();
        if (questLogManager == null) return null;

        var data = new QuestLogSaveData();
        data.actualQuestTextData = questLogManager.actualQuestText.text;
        data.guideTextData = questLogManager.guideText.text;
        return data;
    }

    private ProgressionPointSaveData CaptureProgressionPointData()
    {
        ProgressionPoint progressionPoint = GetComponent<ProgressionPoint>();
        if (progressionPoint == null) return null;

        var data = new ProgressionPointSaveData();
        data.hasBeenActivated = progressionPoint.hasBeenActivated;
        return data;
    }

    private GameManagerSaveData CaptureGameManagerData()
    {
        GameManager gameManager = GetComponent<GameManager>();
        if (gameManager == null) return null;

        var data = new GameManagerSaveData();
        data.gameManagerCanPause = gameManager.canPause;
        data.gameManagerIsFocusing = gameManager.isFocusing;
        data.gameManagerIsHidingUI = gameManager.isHidingUI;
        data.gameManagerIsPaused = gameManager.isPaused;
        return data;
    }

    // Ok after all that capturing is restoring... -----------------------------

    // TODO 6: Then lastly create a restore for the component.
    // AFTER THIS, all the red underlines that appeared earlier shud be gone

    public void RestoreState(ObjectSaveData data)
    {
        // If there's nothing that changed then DON'T SAVE CUH !!!
        if (data == null) return;

        if (data.changedPosition)
        {
            transform.position = data.position;
        }

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

            //if (comp.type == "MultiInteractable"){
            //    var multiInteractable = GetComponent<MultiInteractableObject>();
            //    var compData = comp as MultiInteractableSaveData;

            //    if (multiInteractable != null && compData != null)
            //    {
            //        multiInteractable.hasActivatedOnce = compData.hasActivatedOnce;
            //        multiInteractable.currentInteractions = compData.interactionsCounter;
            //    }
            //}

            if (comp.type == "Oil")
            {
                var oil = GetComponent<Oil>();
                var compData = comp as OilSaveData;

                if (oil != null && compData != null)
                {
                    oil.pH = compData.pH;
                    oil.salinity = compData.salinity;
                    oil.finished = compData.finished;
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
                    //dialogueTrigger.dialogueIsTriggered = compData.dialogueIsTriggered;
                    dialogueTrigger.dialogueIsTriggered = false; // FORCE

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

            if (comp.type == "DialogueLog")
            {
                var logScreen = GetComponent<LogScreen>();
                var compData = comp as DialogueLogSaveData;

                if (logScreen != null && compData != null)
                {
                    logScreen.SetStoredDialogues(compData.storedDialogues);
                }
            }

            if (comp.type == "QuestLog")
            {
                var questLogManager = GetComponent<QuestLogManager>();
                var compData = comp as QuestLogSaveData;

                if (questLogManager != null && compData != null)
                {
                    questLogManager.ChangeQuest(compData.actualQuestTextData, compData.guideTextData);

                    //questLogManager.actualQuestText.text = compData.actualQuestTextData;
                    //questLogManager.guideText.text = compData.guideTextData;
                }
            }

            if (comp.type == "ProgressionPoint")
            {
                var progressionPt = GetComponent<ProgressionPoint>();
                var compData = comp as ProgressionPointSaveData;

                if (progressionPt != null && compData != null)
                {
                    progressionPt.hasBeenActivated = compData.hasBeenActivated;
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

        if (data.changedActive)
        {
            gameObject.SetActive(data.isActive);
        }
    }
}
