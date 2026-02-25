using UnityEngine;
using UnityEditor;

public static class RegenerateIDs
{
    [MenuItem("Tools/Unique ID/Regenerate All IDs In Scene")]
    public static void RegenerateAllIDs()
    {
        var all = Object.FindObjectsByType<UniqueID>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        int count = 0;

        foreach (var uid in all)
        {
            if (PrefabUtility.IsPartOfPrefabAsset(uid)) continue;

            Undo.RecordObject(uid, "Regenerate Unique ID");
            uid.GenerateNewID();
            count++;
        }
    }
}