using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] private Animator animator;

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
    }

    public void TogglePause()
    {
        //active = !active;
        //Time.timeScale = active ? 0 : 1;

        EventBroadcaster.Instance.PostEvent(EventNames.GAME_PAUSED);
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
