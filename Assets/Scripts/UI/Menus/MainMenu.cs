using UnityEngine;

// Currently using this to hold all menu functions
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuDisplay;
    [SerializeField] private GameObject mainMenu_UI; // the menu you want to hide
    [SerializeField] private GameObject loadSave_UI;
    [SerializeField] private GameObject settings_UI;
    [SerializeField] private GameObject gameDisplay;

    // FOR GAME
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject logScreen;

    public void toggleLoadSaveScreen()
    {
        mainMenu_UI.SetActive(!mainMenu_UI.activeInHierarchy);
        loadSave_UI.SetActive(!loadSave_UI.activeInHierarchy);
    }

    public void toggleSettingsScreen()
    {
        mainMenu_UI.SetActive(!mainMenu_UI.activeInHierarchy);
        settings_UI.SetActive(!settings_UI.activeInHierarchy);
    }

    public void goToGame()
    {
        mainMenuDisplay.SetActive(false);
        loadSave_UI.SetActive(false);

        gameDisplay.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
        // REMOVE FOR FINAL BUILD:
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void togglePauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
    }

    public void toggleLogScreen()
    {
        logScreen.SetActive(!logScreen.activeInHierarchy);
    }
}
