using UnityEngine;
using NaughtyAttributes;

public class LockableObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] public bool isLockedAtStart; // Used in ObjectState.cs

    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1;

    [Header("Counters")]
    [ReadOnly, SerializeField] public int currentChecks = 0;

    [Header("Flags")]
    [HideInInspector] public bool isAlreadyLocked = false; // Used in ObjectState.cs
    [HideInInspector] public bool hasStoredLockValue = false; // Used in ObjectState.cs
    [HideInInspector] public bool storedLockValue = false; // Used in ObjectState.cs

    private void Awake()
    {
        if (!isAlreadyLocked)
        {
            Lock(isLockedAtStart);
            isAlreadyLocked = true;
        }
    }

    private void OnEnable()
    {
        if(hasStoredLockValue && currentChecks >= requiredChecks)
        {
            Lock(storedLockValue);
        }
        
    }

    // Helper Functions --------------------------------------------------------

    public void Lock(bool val)
    {
        GetComponent<BoxCollider2D>().enabled = !val;
        hasStoredLockValue = true;
        storedLockValue = val;
    }
}
