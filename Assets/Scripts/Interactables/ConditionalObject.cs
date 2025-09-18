using UnityEngine;

public class ConditionalObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1; // Used in InteractableObject.cs
    [HideInInspector] public int currentChecks = 0; // Used in InteractableObject.cs
}
