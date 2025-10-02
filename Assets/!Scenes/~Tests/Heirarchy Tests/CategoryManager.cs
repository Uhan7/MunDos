using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum CategoryType
{
    None, Past, Present, Canvas, Room, Object, Item, Envi, NPC, Dialogue, Player
}
public class CategoryManager : MonoBehaviour
{
    private Dictionary<CategoryType, List<Category>> categoryMap = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        var allActivators = FindObjectsByType<Category>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
            );
        foreach (var activator in allActivators)
        {
            if (!categoryMap.TryGetValue(activator.category, out var list))
            {
                list = new List<Category>();
                categoryMap[activator.category] = list;
            }
            list.Add(activator);
        }
    }
    public void SetCategoryActive(CategoryType categoryName, bool state)
    {
        
        if (!categoryMap.TryGetValue(categoryName, out var list)) return;


        foreach (var activator in list)
        {
            if (activator.transform.IsChildOf(activator.rootObject.transform)){
                activator.SetActive(state);
            }
            
        }
    }
}
