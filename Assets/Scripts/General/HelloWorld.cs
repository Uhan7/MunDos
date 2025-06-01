using UnityEngine;

public class HelloWorld : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;

    public void PrintHelloWorld()
    {
        print("Hello World!");
    }

    public void SummonMainMenu()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
    }

}
