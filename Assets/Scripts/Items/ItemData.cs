using UnityEngine;

[System.Serializable]

public class ItemData
{
    // Variables
    public string itemName;
    public Sprite itemSprite;
    public GameObject[] objectsToInteractWith;

    // Constructors
    public ItemData() {}

    public ItemData(ItemData other)
    {
        itemName = other.itemName;
        itemSprite = other.itemSprite;
        objectsToInteractWith = other.objectsToInteractWith;
    }
}
