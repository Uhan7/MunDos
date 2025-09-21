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
    [SerializeField] private string zoomEnviContainerName;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;
    [HideInInspector] private DialogueManager dialogueHolderScript;
    [HideInInspector] private PlayerMove protagMoveScript;
    [HideInInspector] private ZoomEnviManager zoomEnviManagerScript;

    [Header("Flags")]
    [HideInInspector] private bool isFocusingDialogue;
    [HideInInspector] private bool isZoomingEnvi;

    private void Awake()
    {
        timelineManagerScript = GetComponent<TimelineManager>();

        EventBroadcaster.Instance.AddObserver(EventNames.FOCUS_DIALOGUE, FocusDialogue);
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
        EventBroadcaster.Instance.AddObserver(EventNames.ZOOM_ENVI, ZoomEnvi);
    }

    // Event Broadcasting Functions --------------------------------------------

    public void FocusDialogue(Parameters param)
    {
        SetComponents();
        isFocusingDialogue = param.GetBoolExtra(ParamNames.IS_FOCUSING_DIALOGUE, false);

        protagMoveScript.canMove = !isFocusingDialogue;
        timelineManagerScript.canSwitch = !isFocusingDialogue;

        if (isZoomingEnvi && !isFocusingDialogue) zoomEnviManagerScript.DeactivateZoomedEnvi();
    }

    public void TogglePause()
    {
        SetComponents();

        pauseMenuScript.active = !pauseMenuScript.active;
        dialogueHolderScript.canClick = !pauseMenuScript.active;

        protagMoveScript.canMove = (!pauseMenuScript.active && !isFocusingDialogue);
        timelineManagerScript.canSwitch = (!pauseMenuScript.active && !isFocusingDialogue);

        Time.timeScale = pauseMenuScript.active ? 0 : 1;
    }

    public void ZoomEnvi(Parameters param)
    {
        SetComponents();

        isZoomingEnvi = param.GetBoolExtra(ParamNames.IS_ZOOMING_ENVI, false);

        if (isZoomingEnvi) zoomEnviManagerScript.ActivateZoomedEnvi(param.GetSpriteData(ParamNames.ZOOM_ENVI_SPRITE));
        else zoomEnviManagerScript.DeactivateZoomedEnvi();
    }

    void SetComponents()
    {
        protagMoveScript = GameObject.FindGameObjectWithTag(PROTAG_TAG).GetComponent<PlayerMove>();
        pauseMenuScript = GameObject.Find(pauseMenuName).GetComponent<PauseMenuManager>();
        dialogueHolderScript = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
        zoomEnviManagerScript = GameObject.Find(zoomEnviContainerName).GetComponent<ZoomEnviManager>();
    }
}
