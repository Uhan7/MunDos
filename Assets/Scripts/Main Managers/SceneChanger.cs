using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private bool loadAfterTime;
    [SerializeField] private float timeToWait;

    private void Start()
    {
        if (loadAfterTime) StartCoroutine(ChangeToScene(sceneToLoad, timeToWait));
    }

    private IEnumerator ChangeToScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
