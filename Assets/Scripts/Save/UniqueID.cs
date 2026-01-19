using UnityEngine;
using System;

[DisallowMultipleComponent]
public class UniqueID : MonoBehaviour
{
    [SerializeField] private string id;

    public string ID => id;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString();
    }
#endif

}
