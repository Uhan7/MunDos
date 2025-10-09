using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] private const string PROTAG_TAG = "Protag";

    [Header("Components")]
    [HideInInspector] private TimelineManager timelineManagerScript;

    [Header("References")]
    [SerializeField] private string pauseMenuName;
    [SerializeField] private string dialogueHolderName;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;
    [HideInInspector] private DialogueManager dialogueHolderScript;
    [HideInInspector] private PlayerMove protagMoveScript;

    [Header("Flags")]
    [HideInInspector] private bool isFocusingDialogue;

    private void Awake()
    {
        timelineManagerScript = GetComponent<TimelineManager>();

        EventBroadcaster.Instance.AddObserver(EventNames.FOCUS_DIALOGUE, FocusDialogue);
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
    }

    // Event Broadcasting Functions --------------------------------------------

    public void FocusDialogue(Parameters param)
    {
        SetComponents();
        isFocusingDialogue = param.GetBoolExtra(ParamNames.IS_FOCUSING_DIALOGUE, false);

        protagMoveScript.canMove = !isFocusingDialogue;
        protagMoveScript.canInput = !isFocusingDialogue;
        timelineManagerScript.canSwitch = !isFocusingDialogue;
    }

    public void TogglePause()
    {
        SetComponents();

        pauseMenuScript.active = !pauseMenuScript.active;
        dialogueHolderScript.canClick = !pauseMenuScript.active;

        protagMoveScript.canMove = (!pauseMenuScript.active && !isFocusingDialogue);
        protagMoveScript.canInput = (!pauseMenuScript.active && !isFocusingDialogue);
        timelineManagerScript.canSwitch = (!pauseMenuScript.active && !isFocusingDialogue);

        Time.timeScale = pauseMenuScript.active ? 0 : 1;
    }

    void SetComponents()
    {
        protagMoveScript = GameObject.FindGameObjectWithTag(PROTAG_TAG).GetComponent<PlayerMove>();
        pauseMenuScript = GameObject.Find(pauseMenuName).GetComponent<PauseMenuManager>();
        dialogueHolderScript = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
    }
}
