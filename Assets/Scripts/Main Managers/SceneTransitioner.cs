using System.Collections;
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
        StartCoroutine(GoToScene());
    }

    private IEnumerator GoToScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
