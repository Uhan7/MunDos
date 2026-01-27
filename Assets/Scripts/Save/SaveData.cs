using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<ObjectSaveData> objectStates;
    public List<PlayerItemSaveData> playerItems;
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
}

[Serializable]
public class PlayerItemSaveData
{
    public Timeline protagTimeline;
    public List<ItemData> itemDatas;
    public int playerItemIndex;
}