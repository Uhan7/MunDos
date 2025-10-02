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
        Debug.Log($"[CategoryManager] Activating category: {categoryName}, state: {state}");
        if (!categoryMap.TryGetValue(categoryName, out var list))
        {
            Debug.LogWarning($"[CategoryManger] No activators found for category: {categoryName}");
            return;
        }


        foreach (var activator in list)
        {
            if (activator == null)
            {
                Debug.LogWarning("[CategoryManager] Skipping null activator in list.");
                continue;
            }
            if (activator.rootObject == null)
            {
                Debug.LogWarning($"[CategoryManager] Activator '{activator.gameObject.name}' has no rootObject set.");
                continue;
            }

            if (activator.transform.IsChildOf(activator.rootObject.transform)){
                Debug.Log($"[CategoryManager] Setting '{activator.gameObject.name}' active = {state}");

                activator.SetActive(state);
            }
            else
            {
                Debug.Log($"[CategoryManager] Skipping '{activator.gameObject.name}' — not under root '{activator.rootObject.name}'");
            }
        }
    }
}

