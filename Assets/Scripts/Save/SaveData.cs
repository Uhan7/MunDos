using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<ObjectSaveData> objectStates;
    public List<PlayerItemSaveData> playerItems;

    public bool timelineUnlocked; // Temporary fix,,, make a dictionary of this eventually
}

[Serializable]
public class ObjectSaveData
{
    // Envi, Item, NPC, Room, DZ, Timeline, Bascally everything lol
    public string objectID;

    // If need to check if an object is active
    public bool isActive;

    // If need to check object's position
    public Vector3 position;

    // If need to check object's colliders
    public bool colliderState;
}

[Serializable]
public class PlayerItemSaveData
{
    public Timeline protagTimeline;
    public List<ItemData> itemDatas;
    public int playerItemIndex;
}