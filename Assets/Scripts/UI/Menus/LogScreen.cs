using UnityEngine;

public class LogScreen : MonoBehaviour
{
    [SerializeField] private GameObject logScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void toggleLogScreen()
    {
        logScreen.SetActive(!logScreen.activeInHierarchy);
    }
}
