using UnityEngine;

public class ObjectState : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
}