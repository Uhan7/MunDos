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

    // Basic Object Stuff
    public bool isActive;
    public Vector3 position;
    public bool colliderState;

    // Conditional
    public int conditionalObjectChecks;

    // Lock
    public bool isLockedAtStart;
    public bool isAlreadyLocked;
    public int lockableObjectChecks;
    public bool hasStoredLockValue;
    public bool storedLockValue;

    // Already Checked Flags
    public bool interactableAlreadyCheckedConditionals;
    public bool interactableAlreadyCheckedUnlock;
    public bool interactableAlreadyCheckedLock;
}

[Serializable]
public class PlayerItemSaveData
{
    public Timeline protagTimeline;
    public List<ItemData> itemDatas;
    public int playerItemIndex;
}