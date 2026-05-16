using UnityEngine;

[System.Serializable]

public class ItemData
{

    // Variables
    public string itemName;
    public Sprite itemSprite;
    public Timeline timeline;
    public GameObject[] objectsToInteractWith; // maybe replace dis wit ID?

    // Constructors
    public ItemData() {}

    public ItemData(ItemData other)
    {
        itemName = other.itemName;
        itemSprite = other.itemSprite;
        timeline = other.timeline;
        objectsToInteractWith = other.objectsToInteractWith;
    }
}
