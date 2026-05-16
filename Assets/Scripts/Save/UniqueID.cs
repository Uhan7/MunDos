using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent] [ExecuteAlways]
public class UniqueID : MonoBehaviour
{
    [SerializeField] private string id;

    public string ID => id;

    public void GenerateNewID()
    {
        id = Guid.NewGuid().ToString();
    }

    public string GetID()
    {
        return id;
    }


    /*

    Bru fuk all this lol

    private void Awake()
    {
        EnsureIDExists();
    }

    private void EnsureIDExists()
    {
        if (!string.IsNullOrEmpty(id))
            return;

        GenerateNewID();
    }

    public void GenerateNewID()
    {
        id = Guid.NewGuid().ToString();
#if UNITY_EDITOR
        MarkDirty();
#endif

    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (PrefabUtility.IsPartOfPrefabAsset(this))
        {
            id = string.Empty;
            return;
        }

        EnsureIDExists();
    }

    private void MarkDirty()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this);
        }
    }
#endif

    */


}