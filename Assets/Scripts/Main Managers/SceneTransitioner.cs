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

    private Animator transitionAnimator;

    private void Start()
    {
        transitionAnimator = transitionObject.GetComponent<Animator>();  
    }

    public void SceneTransitionWrapper(string _sceneName)
    {
        transitionObject.SetActive(true);
        transitionAnimator.SetBool("Loading", true);

        sceneName = _sceneName;

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
