using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScrollImage : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float positionToStopIn;

    [HideInInspector] private bool startedFadeOut;
    [SerializeField] private Image blackScreen;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        startedFadeOut = false;
        blackScreen.canvasRenderer.SetAlpha(0f);
    }

    private void FixedUpdate()
    {
        if (GetComponent<RectTransform>().anchoredPosition.y < positionToStopIn) transform.position = new Vector3(transform.position.x, transform.position.y + scrollSpeed * Time.deltaTime, transform.position.z);
        else
        {
            if (!startedFadeOut)
            {
                blackScreen.CrossFadeAlpha(1.0f, 3.5f, false);
                Invoke("LoadTitleScreen", 4.5f);
                StartCoroutine(FadeOutAudioSource(4f));
                startedFadeOut = true;
            }
        }
    }

    private void LoadTitleScreen()
    {
        SceneManager.LoadScene("Title Screen");
    }

    private IEnumerator FadeOutAudioSource(float duration)
    {
        float startVolume = audioSource.volume;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.volume = startVolume;
    }
}
