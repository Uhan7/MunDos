using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [Header("Extra Variables")]
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private float transitionDelay;

    [HideInInspector] private string sceneName;

    public void SceneTransitionWrapper(string _sceneName)
    {
        sceneName = _sceneName;

        transitionObject.SetActive(true);
        Invoke("GoToScene", transitionDelay);
    }

    private void GoToScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
