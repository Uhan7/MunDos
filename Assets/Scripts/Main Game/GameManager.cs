using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string pauseMenuName;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
    }

    public void TogglePause()
    {
        pauseMenuScript = GameObject.Find(pauseMenuName).GetComponent<PauseMenuManager>();
        pauseMenuScript.active = !pauseMenuScript.active;
        Time.timeScale = pauseMenuScript.active ? 0 : 1;
    }
}
