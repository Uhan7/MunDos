using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<ObjectSaveData> objectStates;
    public List<PlayerItemSaveData> playerItems;

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

    // ObjectsManager
    public bool objectManagerAlreadyEnabled;
    public bool objectManagerAlreadyCheckedUnlock;
    public bool objectManagerAlreadyCheckedLock;

    // ZoomEnviManager
    public bool zoomEnviManagerActivated;

    // Game Manager (this should actually be elsewhere but ourgh idc)
    public bool gameManagerIsFocusing;
    public bool gameManagerIsHidingUI;
    public bool gameManagerIsPaused;
    public bool gameManagerCanPause;
}

[Serializable]
public class PlayerItemSaveData
{
    public Timeline protagTimeline;
    public List<ItemData> itemDatas;
    public int playerItemIndex;
}