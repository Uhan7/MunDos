using UnityEngine;

public class ConditionalObject : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public int requiredChecks; // Used in ConiditionalObjectsManager.cs
    [HideInInspector] public int currentChecks; // Used in ConiditionalObjectsManager.cs
}
