using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<ObjectSaveData> objectStates;
    public List<PlayerSaveData> players;

    public bool timelineUnlocked; // Temporary fix,,, make a dictionary of this (Game States) eventually
    public int currentTimeline; // another temporary, 0 is past and 1 is present
}

[Serializable]
public class ObjectSaveData
{
    // Envi, Item, NPC, Room, DZ, Timeline, Bascally everything lol
    public string objectID;

    // GameObject
    public bool isActive;

    // Transform
    public Vector3 position;

    // Collider
    public bool colliderState;

    // Conditional
    public int conditionalObjectChecks;

    // Lock
    public bool isLockedAtStart;
    public bool isAlreadyLocked;
    public int lockableObjectChecks;
    public bool hasStoredLockValue;
    public bool storedLockValue;

    // Interactable
    public bool interactableAlreadyCheckedConditionals;
    public bool interactableAlreadyCheckedUnlock;
    public bool interactableAlreadyCheckedLock;
    public bool interactableIsFocusing;

    // DialogueTrigger
    public bool dialogueIsTriggered;
    public bool dialogueAlreadyActivatedObjects;
    public bool dialogueAlreadyDeactivatedObjects;
    public bool dialogueAlreadyCheckedConditional;
    public bool dialogueAlreadyRemovedPlayeritems;
    public bool dialogueAlreadyGavePlayeritems;

    // Auto Move Zone
    public AutoMove.Direction autoMoveZoneDirection;

    // ObjectsManager
    public bool objectManagerAlreadyEnabled;
    public bool objectManagerAlreadyCheckedUnlock;
    public bool objectManagerAlreadyCheckedLock;

    // ZoomEnviManager
    public bool zoomEnviManagerActivated;

    // DialogueManager
    public bool dialogueManagerWasClicked;
    public bool dialogueManagerOpen;
    public bool dialogueManagerSkip;
    public bool dialogueManagerCanNext;
    public bool dialogueManagerCanClick;
    public bool dialogueManagerMainCharacterIsSpeaking;

    // Game Manager (this should actually be elsewhere but ourgh idc)
    public bool gameManagerIsFocusing;
    public bool gameManagerIsHidingUI;
    public bool gameManagerIsPaused;
    public bool gameManagerCanPause;
}

[Serializable]
public class PlayerSaveData
{
    // Current Timeline (will load this timeline on load)
    public Timeline protagTimeline;

    // Player Items
    public List<ItemData> itemDatas;
    public int playerItemIndex;

    // Player Movement Flags
    public bool moveCanInput;
    public bool moveCanMove;

    // Player Interaction Flags
    public bool interactWillUpdate;
    public bool interactIsEmpty;
    public bool interactHasCheckedInventory;
    public bool interactCanInput;
    public bool interactHasActiveItem;
    public bool interactIsSelected;
    public bool interactIsZoomed;
    public int interactActiveObjectIndex;
}