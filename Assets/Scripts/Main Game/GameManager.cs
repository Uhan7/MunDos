using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pastPauseMenu;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pastPauseMenuScript;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);

        pastPauseMenuScript = pastPauseMenu.GetComponent<PauseMenuManager>();
    }

    public void TogglePause()
    {
        pastPauseMenuScript.active = !pastPauseMenuScript.active;
        Time.timeScale = pastPauseMenuScript.active ? 0 : 1;
    }
}
