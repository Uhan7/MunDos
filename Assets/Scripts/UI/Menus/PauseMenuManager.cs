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
        animator.SetBool("Active", active);

        if (Input.GetKeyDown(pauseKey) && gameManager.canPause) PauseScreenOn();
        else if (Input.GetKeyDown(pauseKey) && !gameManager.canPause) PauseScreenOff();
    }

    public void TogglePause()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE);
    }

    public void PauseScreenOn()
    {
        if (gameManager.canPause && !active)
        {
            gameManager.canPause = false;
            TogglePause();
        }
    }

    public void PauseScreenOff()
    {
        if (active && IsAllWindowsClosed())
        {
            gameManager.canPause = true;
            TogglePause();
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
