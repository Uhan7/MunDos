using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using NaughtyAttributes;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;
    [SerializeField] private AudioSource areaBGMSource;
    [SerializeField] private GameObject[] parallaxObjects;
    [SerializeField] private bool showFirst;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = col.gameObject.transform;

        roomCamera.SetActive(true);
        if (areaBGMSource != null) StartCoroutine(FadeInBGM(areaBGMSource, 2f));

        if (parallaxObjects.Length > 0) foreach (GameObject parallaxObject in parallaxObjects)
        {
                if (!showFirst) parallaxObject.SetActive(false);
                else showFirst = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(false);
        if (areaBGMSource != null) StartCoroutine(FadeOutBGM(areaBGMSource, 2f));
    }

    // Helper Functions and Coroutines

    IEnumerator FadeOutBGM(AudioSource bgmSource, float fadeDuration)
    {
        float time = 0f;
        float startVolOut = bgmSource.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            bgmSource.volume = Mathf.Lerp(startVolOut, 0f, t);

            yield return null;
        }

        bgmSource.volume = 0f;
    }

    IEnumerator FadeInBGM(AudioSource bgmSource, float fadeDuration)
    {
        float time = 0f;
        float startVolIn = bgmSource.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            bgmSource.volume = Mathf.Lerp(startVolIn, SettingsInfo.musicVol, t);

            yield return null;
        }

        bgmSource.volume = SettingsInfo.musicVol;
    }
}
