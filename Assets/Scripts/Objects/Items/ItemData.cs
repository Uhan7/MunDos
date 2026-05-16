using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class ItemData
{

    // Variables
    public string itemName;
    public Sprite itemSprite;
    public Timeline timeline;
    public GameObject[] objectsToInteractWith;
    public string[] objectIDsToInteractWith;

    // Constructors
    public ItemData() {}

    public ItemData(ItemData other)
    {
        itemName = other.itemName;
        itemSprite = other.itemSprite;
        timeline = other.timeline;
        objectsToInteractWith = other.objectsToInteractWith;
        objectIDsToInteractWith = other.objectIDsToInteractWith;
    }
}
