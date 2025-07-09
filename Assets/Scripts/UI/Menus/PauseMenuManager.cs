using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject settingsUI;

    private Animator animator;
    private bool active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Active", active);
    }

    public void TogglePause()
    {
        active = !active;
        Time.timeScale = active ? 0 : 1;
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
