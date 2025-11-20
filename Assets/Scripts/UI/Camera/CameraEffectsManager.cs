using UnityEngine;
using UnityEngine.UI;

public class CameraEffectsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject flashObject;
    [SerializeField] private GameObject dimObject;

    [Header("Visual Effects SFX")]
    [SerializeField] private AudioClip shakeSFX;
    [SerializeField] private AudioClip flashSFX;
    [SerializeField] private AudioClip dimSFX;

    [Header("Voices SFX")]
    [SerializeField] private AudioClip santiConcernedSFX;
    [SerializeField] private AudioClip santiFrustratedSFX;
    [SerializeField] private AudioClip santiHappySFX;
    [SerializeField] private AudioClip santiOhSFX;
    [SerializeField] private AudioClip santiQuestioningSFX;
    [SerializeField] private AudioClip santiRelievedSFX;
    [SerializeField] private AudioClip santiSatisfiedSFX;
    [SerializeField] private AudioClip santiGaspSFX;
    [SerializeField] private AudioClip santiSighSFX;
    [SerializeField] private AudioClip santiThinkingSFX;
    [SerializeField] private AudioClip santiWonderingSFX;

    // Helper Functions --------------------------------------------------------

    public void ShakeScreen()
    {
        mainCamera.GetComponent<CameraShake>().ScreenShakeWrapper();
        sfxSource.PlayOneShot(shakeSFX);
    }

    public void StartShakeScreen()
    {
        mainCamera.GetComponent<CameraShake>().StartScreenShakeWrapper();
        sfxSource.PlayOneShot(shakeSFX);
    }

    public void EndShakeScreen()
    {
        mainCamera.GetComponent<CameraShake>().EndScreenShakeWrapper();
    }

    public void Flash()
    {
        //flashObject.GetComponent<Animator>().Play("image_fade_out_half");
        flashObject.GetComponent<Fader>().StopAllCoroutines();

        flashObject.GetComponent<Fader>().FadeSequence(1, 0.05f, 0, 0.45f);
        sfxSource.PlayOneShot(flashSFX);
    }

    public void StartFlash()
    {
        //flashObject.GetComponent<Animator>().Play("image_fade_in_half");
        flashObject.GetComponent<Fader>().StopAllCoroutines();

        flashObject.GetComponent<Fader>().FadeToWrapper(1, 2.5f);
        sfxSource.PlayOneShot(flashSFX);
    }

    public void EndFlash()
    {
        //flashObject.GetComponent<Animator>().Play("image_fade_out_half");
        flashObject.GetComponent<Fader>().StopAllCoroutines();

        flashObject.GetComponent<Fader>().FadeToWrapper(0, 1);
    }

    public void Dim()
    {
        //dimObject.GetComponent<Animator>().Play("image_fade_out_half");
        dimObject.GetComponent<Fader>().StopAllCoroutines();

        dimObject.GetComponent<Fader>().FadeSequence(1, 0.05f, 0, 0.45f);
        sfxSource.PlayOneShot(dimSFX);
    }

    public void StartDim()
    {
        //dimObject.GetComponent<Animator>().Play("image_fade_in_half");
        dimObject.GetComponent<Fader>().StopAllCoroutines();

        dimObject.GetComponent<Fader>().FadeToWrapper(1, 2.5f);
        sfxSource.PlayOneShot(dimSFX);
    }

    public void EndDim()
    {
        //dimObject.GetComponent<Animator>().Play("image_fade_out_half");
        dimObject.GetComponent<Fader>().StopAllCoroutines();

        dimObject.GetComponent<Fader>().FadeToWrapper(0, 1);
    }

    public void EndAll()
    {
        flashObject.GetComponent<Fader>().StopAllCoroutines();
        dimObject.GetComponent<Fader>().StopAllCoroutines();

        flashObject.GetComponent<Image>().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        dimObject.GetComponent<Image>().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        mainCamera.GetComponent<CameraShake>().EndScreenShakeWrapper();
    }

    public void PlaySFX(string audioClipName)
    {
        switch (audioClipName)
        {
            case "<SFX_santi_concerned>": sfxSource.PlayOneShot(santiConcernedSFX); break;
            default: break;
        }
    }
}
