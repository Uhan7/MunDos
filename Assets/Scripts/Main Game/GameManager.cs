using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string pauseMenuName;
    [SerializeField] private string dialogueHolderName;
    [SerializeField] private string protagTag;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;
    [HideInInspector] private PlayerMove protagMoveScript;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
        EventBroadcaster.Instance.AddObserver(EventNames.FOCUS_DIALOGUE, TogglePause);
    }

    // Event Broadcasting Functions --------------------------------------------

    public void TogglePause()
    {
        pauseMenuScript = GameObject.Find(pauseMenuName).GetComponent<PauseMenuManager>();
        pauseMenuScript.active = !pauseMenuScript.active;
        Time.timeScale = pauseMenuScript.active ? 0 : 1;
    }

    public void FocusDialogue(Parameters param)
    {

    }
}
