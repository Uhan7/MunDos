using UnityEngine;

public class TimelineUnlocker : MonoBehaviour
{
    [SerializeField] private TimelineManager timelineManager;

    private void OnEnable()
    {
        timelineManager.timelineUnlocked = true;
        Destroy(gameObject);
    }
}
