using UnityEngine;

public class GameMenus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject mainMenuDisplay;
    [SerializeField] private GameObject mainMenu_UI;
    [SerializeField] private GameObject gameDisplay;

    private void Start()
    {
        Debug.Log(gameObject);
    }

    public void goToMainMenu()
    {
        mainMenu_UI.SetActive(true);
        mainMenuDisplay.SetActive(true);

        gameDisplay.SetActive(false);
    }
}
