using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Animator animator;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode pauseKey;

    [Header("Window References")]
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject settingsUI;

    [Header("Flags")]
    [HideInInspector] public bool active = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Active", active);

        if (Input.GetKeyDown(pauseKey)) TogglePause();
    }

    public void TogglePause()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_PAUSE);
    }

    public void toggleSettingsScreen()
    {
        settingsUI.SetActive(!settingsUI.activeInHierarchy);
    }

    public void toggleSaveScreen()
    {
        saveUI.SetActive(!saveUI.activeInHierarchy);
    }
}
