using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<ProtagSaveData> protagStates;
    public List<ObjectSaveData> objectStates;
}

[Serializable]
public class ProtagSaveData
{
    public string protagID;
    public Vector3 protagPosition;
    public List<string> protagInventoryItemIDs;
}

[Serializable]
public class ObjectSaveData
{
    // Envi, Item, NPC, Room, DZ, Timeline, Bascally everything lol

    public string objectID;
    public bool isActive;
}