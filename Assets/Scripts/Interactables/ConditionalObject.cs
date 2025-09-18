using UnityEngine;

public class ConditionalObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public int requiredChecks; // Used in ConditionalObjectsManager.cs
    [HideInInspector] public int currentChecks; // Used in ConditionalObjectsManager.cs
}
