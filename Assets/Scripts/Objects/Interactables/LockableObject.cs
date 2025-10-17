using UnityEngine;

public class LockableObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isLockedAtStart;

    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1;
    [HideInInspector] public int currentChecks = 0;

    [Header("Flags")]
    [HideInInspector] private bool isAlreadyLocked = false;
    [HideInInspector] private bool hasStoredLockValue = false;
    [HideInInspector] private bool storedLockValue = false;

    private void OnEnable()
    {
        if (!isAlreadyLocked)
        {
            Lock(isLockedAtStart);
            isAlreadyLocked = true;
        }
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
