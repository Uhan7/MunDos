using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Animator animator;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode pauseKey;

    [Header("References")]
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameManager gameManager;

    [Header("Flags")]
    [HideInInspector] public bool active = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (active && !SettingsInfo.timePaused)
        {
            active = false;
        }
        animator.SetBool("Active", active);

        if (gameManager.isHidingUI) return;
        if (Input.GetKey(KeyCode.Tab)) return;

        if (Input.GetKeyDown(pauseKey) && gameManager.canPause) PauseScreenOn();
        else if (Input.GetKeyDown(pauseKey) && !gameManager.canPause) PauseScreenOff();
    }

    public void TogglePause()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE);
    }

    public void PauseScreenOn()
    {
        if (!active && SettingsInfo.timePaused)
        {
            Debug.LogError("Time and pause mismatch 1: [PauseScreenOn()] Trying to activate pause screen but time is paused.");
            active = true;
            EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE_BG);
        }
        if (gameManager.canPause && !active)
        {
            gameManager.canPause = false;
            if (!SettingsInfo.timePaused) TogglePause();
        }
    }

    public void PauseScreenOff()
    {
        if (active && !SettingsInfo.timePaused)
        {
            Debug.LogError("Time and pause mismatch 2: [PauseScreenOff()] Trying to deactivate pause screen but time is already unpaused.");
            gameManager.canPause = true;
            TogglePause();
        }
        if (active && IsAllWindowsClosed())
        {
            gameManager.canPause = true;
            if (SettingsInfo.timePaused) TogglePause();
        }
    }

    public void toggleSettingsScreen()
    {
        settingsUI.SetActive(!settingsUI.activeInHierarchy);
    }

    public void toggleSaveScreen()
    {
        saveUI.SetActive(!saveUI.activeInHierarchy);
    }

    // Helper functions ---------------------------------
    private bool IsAllWindowsClosed()
    {
        return !(saveUI.activeInHierarchy || settingsUI.activeInHierarchy);
    }
}
