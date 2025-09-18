using UnityEngine;

public class ConditionalObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1; // Used in ConditionalObjectsManager.cs
    [HideInInspector] public int currentChecks = 0; // Used in ConditionalObjectsManager.cs
}
