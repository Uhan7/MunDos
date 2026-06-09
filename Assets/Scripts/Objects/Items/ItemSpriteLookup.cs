using System.Collections.Generic;
using UnityEngine;

public class ItemSpriteLookup : MonoBehaviour
{
    [Header("Singleton")]
    [HideInInspector] public static ItemSpriteLookup Instance;

    [Header("Content")]
    [SerializeField] private List<ItemEntry> entries = new();

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSprite(string itemName)
    {
        foreach (ItemEntry entry in entries)
        {
            if (entry.itemName == itemName) return entry.sprite;
        }

        Debug.LogWarning("No sprite found for " + itemName);
        return null;
    }
}