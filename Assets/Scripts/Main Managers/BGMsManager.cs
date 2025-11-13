using System.Collections;
using UnityEngine;

public class BGMsManager : MonoBehaviour
{
    // [SerializeField] private GameObject[] roomCameras;
    // [SerializeField] private GameObject[] bgmSources;

    public void ChangeBGMWrapper(GameObject room, bool val)
    {
        if (val == true) StartCoroutine(FadeInBGM(room.GetComponent<Room>().connectedAudioSource, 2f));
        if (val == false) StartCoroutine(FadeOutBGM(room.GetComponent<Room>().connectedAudioSource, 2f));
    }

    // Helper Functions

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
}
