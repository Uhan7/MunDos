using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] private const string PROTAG_TAG = "Protag";

    [Header("References")]
    [SerializeField] private string pauseMenuName;
    [SerializeField] private string dialogueHolderName;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;
    [HideInInspector] private PlayerMove protagMoveScript;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
        EventBroadcaster.Instance.AddObserver(EventNames.FOCUS_DIALOGUE, FocusDialogue);
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
        if (protagMoveScript == null) protagMoveScript = GameObject.FindGameObjectWithTag(PROTAG_TAG).GetComponent<PlayerMove>();

        protagMoveScript.canMove = param.GetBoolExtra(ParamNames.IS_FOCUSING_DIALOGUE, false);
    }
}
