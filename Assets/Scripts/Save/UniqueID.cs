using UnityEngine;
using System;

using UnityEditor;
// using UnityEditor.SceneManagement;

[DisallowMultipleComponent]
public class UniqueID : MonoBehaviour
{
    [SerializeField] private string id;
    public string ID => id;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
   //     if (PrefabStageUtility.GetPrefabStage(gameObject) != null) return;

        if (string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString();
    }
#endif
}
