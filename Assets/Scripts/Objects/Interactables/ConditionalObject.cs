using UnityEngine;
using NaughtyAttributes;

public class ConditionalObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1; // Used in InteractableObject.cs
    [ReadOnly] public int currentChecks = 0; // Used in InteractableObject.cs
    
}
