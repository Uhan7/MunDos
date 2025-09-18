using UnityEngine;

public class LockableObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isLockedAtStart;

    [Header("Variables")]
    [SerializeField] public int requiredChecks = 1;
    [HideInInspector] public int currentChecks = 0;

    private void OnEnable()
    {
        Lock(isLockedAtStart);
    }

    // Helper Functions --------------------------------------------------------

    public void Lock(bool val)
    {
        GetComponent<BoxCollider2D>().enabled = !val;
    }
}
