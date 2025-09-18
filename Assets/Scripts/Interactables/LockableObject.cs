using UnityEngine;

public class LockableObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isLockedAtStart;

    private void Start()
    {
        Lock(isLockedAtStart);
    }

    // Helper Functions --------------------------------------------------------

    public void Lock(bool val)
    {
        GetComponent<BoxCollider2D>().enabled = !val;
    }
}
