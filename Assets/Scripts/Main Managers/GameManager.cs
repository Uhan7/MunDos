using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] private const string PROTAG_TAG = "Protag";

    [Header("Components")]
    [HideInInspector] private TimelineManager timelineManagerScript;

    [Header("References")]
    [SerializeField] private string pauseMenuName;
    [SerializeField] private string dialogueHolderName;
    [SerializeField] private GameObject[] UIToHide;
    [SerializeField] private AudioSource audioSource;

    [Header("On Start")]
    [SerializeField] private GameObject globalLight;
    [SerializeField] private GameObject coolerLight;

    [Header("Referenced Components")]
    [HideInInspector] private PauseMenuManager pauseMenuScript;
    [HideInInspector] private DialogueManager dialogueHolderScript;
    [HideInInspector] private PlayerMove protagMoveScript;
    [HideInInspector] private PlayerInteract playerInteractScript;

    [Header("Extra Effects")]
    [SerializeField] private AudioClip pauseOpenSFX;
    [SerializeField] private AudioClip pauseCloseSFX;

    [Header("Flags")]
    [SerializeField, ReadOnly] public bool isFocusing;
    [SerializeField, ReadOnly] public bool isHidingUI;
    [SerializeField, ReadOnly] public bool isPaused = false;
    [SerializeField, ReadOnly] public bool canPause = true;

    [Header("Debug")]
    [SerializeField] private bool overrideDebugMode = false;

    private void Awake()
    {
        timelineManagerScript = GetComponent<TimelineManager>();

        EventBroadcaster.Instance.AddObserver(EventNames.FOCUS_DIALOGUE, FocusDialogue);
        EventBroadcaster.Instance.AddObserver(EventNames.HIDE_UI, HideUI);
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE, TogglePause);
        EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_PAUSE_BG, TogglePauseBackground);
    }

    private void Start()
    {
        if (overrideDebugMode) SettingsInfo.debugMode = overrideDebugMode;

        globalLight.SetActive(false);
        coolerLight.SetActive(true);

        if (SettingsInfo.fromContinue)
        {
            ResetAllPauseVars();
        }
    }

    private void Update()
    {
        if (SettingsInfo.debugMode == false) return;

        if (Input.GetKeyDown(KeyCode.Equals)) Time.timeScale += 0.25f;
        if (Input.GetKeyDown(KeyCode.Minus)) Time.timeScale -= 0.25f;
        if (Input.GetKeyDown(KeyCode.Alpha0)) Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.FOCUS_DIALOGUE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.HIDE_UI);
        EventBroadcaster.Instance.RemoveObserver(EventNames.TOGGLE_PAUSE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.TOGGLE_PAUSE_BG);
    }

    // Event Broadcasting Functions --------------------------------------------

    public void FocusDialogue(Parameters param)
    {
        SetComponents();
        isFocusing = param.GetBoolExtra(ParamNames.IS_FOCUSING_DIALOGUE, false);

        protagMoveScript.canMove = !isFocusing;
        protagMoveScript.canInput = !isFocusing;
        timelineManagerScript.canSwitch = !isFocusing;
    }

    public void HideUI(Parameters param)
    {
        SetComponents();
        isHidingUI = param.GetBoolExtra(ParamNames.IS_HIDING_UI, false);

        if (!isHidingUI)
        {
            foreach (GameObject obj in UIToHide)
            {
                obj.GetComponent<Animator>().ResetTrigger("FadeOut");
                obj.GetComponent<Animator>().SetTrigger("FadeIn");
            }
        } else
        {
            foreach (GameObject obj in UIToHide)
            {
                obj.GetComponent<Animator>().ResetTrigger("FadeIn");
                obj.GetComponent<Animator>().SetTrigger("FadeOut");
            }
        }

        timelineManagerScript.canSwitch = !isHidingUI;
    }

    public void TogglePause()
    {
        if (isHidingUI) return;

        TogglePauseBackground();

        // Force the item zoom in to state 4: unzoom
        playerInteractScript.ToggleStates(playerInteractScript.playerItemIndex, 4);

        audioSource.PlayOneShot(isPaused ? pauseOpenSFX : pauseCloseSFX);

        pauseMenuScript.active = !pauseMenuScript.active;
    }

    public void TogglePauseBackground()
    {
        SetComponents();

        isPaused = !isPaused;
        dialogueHolderScript.canClick = !isPaused;

        protagMoveScript.canMove = (!isPaused && !isFocusing);
        protagMoveScript.canInput = (!isPaused && !isFocusing);
        timelineManagerScript.canSwitch = (!isPaused && !isFocusing);
        playerInteractScript.canInput = (!isPaused);

        SettingsInfo.timePaused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResetAllPauseVars()
    {
        SetComponents();

        isPaused = false;
        isFocusing = false;
        isHidingUI = false;

        dialogueHolderScript.canClick = true;

        foreach (GameObject obj in UIToHide)
        {
            Animator uiAnimator = obj.GetComponent<Animator>();
            if (uiAnimator != null)
            {
                uiAnimator.ResetTrigger("FadeOut");
                uiAnimator.SetTrigger("FadeIn");
            }
        }

        if (!dialogueHolderScript.open)
        {
            protagMoveScript.canMove = true;
            protagMoveScript.canInput = true;
        }

        timelineManagerScript.canSwitch = (!isPaused && !isFocusing);
        playerInteractScript.canInput = (!isPaused);
        canPause = true;

        SettingsInfo.timePaused = false;
        Time.timeScale = 1;
    }

    void SetComponents()
    {
        protagMoveScript = GameObject.FindGameObjectWithTag(PROTAG_TAG).GetComponent<PlayerMove>();
        pauseMenuScript = GameObject.Find(pauseMenuName).GetComponent<PauseMenuManager>();
        dialogueHolderScript = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
        playerInteractScript = GameObject.FindGameObjectWithTag(PROTAG_TAG).GetComponent<PlayerInteract>();
    }
}
