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

    public float totalPlayTime;
}

[Serializable]
public class ObjectSaveData
{
    // Envi, Item, NPC, Room, DZ, Timeline, Bascally everything lol
    public string objectID;

    // GameObject
    public bool isActive;
    public bool changedActive;

    // Transform
    public Vector3 position;
    public bool changedPosition;

    // Other Components
    [SerializeReference] public List<ComponentSaveData> components = new();
}

[Serializable]
public class PlayerSaveData
{
    // Current Timeline (will load this timeline on load)
    public Timeline protagTimeline;

    // Player Items
    [SerializeReference] public List<ItemData> itemDatas;
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