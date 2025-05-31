using UnityEngine;

// Holds functions for controlling button functionalities on the MAIN MENU 
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuDisplay;
    [SerializeField] private GameObject mainMenu_UI;
    [SerializeField] private GameObject loadSave_UI;
    [SerializeField] private GameObject settings_UI;
    [SerializeField] private GameObject gameDisplay;

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
}
