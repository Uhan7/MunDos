using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitioner : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private Image loadingCircle;

    [Header("Variables")]
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

    public void TriggerLoadingScreen()
    {
        transitionObject.SetActive(true);
        transitionAnimator.SetBool("Fake", true);
        transitionAnimator.SetBool("Loading", true);
    }

    public void EndLoadingScreen()
    {
        transitionAnimator.SetBool("Loading", false);
        transitionAnimator.SetBool("Fake", false);
        transitionObject.SetActive(false);
    }

    private IEnumerator GoToScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingCircle.fillAmount = loadProgress;
            yield return null;
        }
    }


}
