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

    [Header("Voices SFX: Santi")]
    [SerializeField] private AudioClip santiConcernedSFX;
    [SerializeField] private AudioClip santiFrustratedSFX;
    [SerializeField] private AudioClip santiHappySFX;
    [SerializeField] private AudioClip santiOhSFX;
    [SerializeField] private AudioClip santiQuestioningSFX;
    [SerializeField] private AudioClip santiRelievedSFX;
    [SerializeField] private AudioClip santiSatisfiedSFX;
    [SerializeField] private AudioClip santiShockGaspSFX;
    [SerializeField] private AudioClip santiSighSFX;
    [SerializeField] private AudioClip santiThinkingSFX;
    [SerializeField] private AudioClip santiWonderingSFX;

    [Header("Voices SFX: Liezel")]
    [SerializeField] private AudioClip liezelConcernedSFX;
    [SerializeField] private AudioClip liezelDiscoverySFX;
    [SerializeField] private AudioClip liezelFrustratedSFX;
    [SerializeField] private AudioClip liezelHappySFX;
    [SerializeField] private AudioClip liezelOhSFX;
    [SerializeField] private AudioClip liezelQuestioningSFX;
    [SerializeField] private AudioClip liezelRelievedSFX;
    [SerializeField] private AudioClip liezelSatisfiedAhaSFX;
    [SerializeField] private AudioClip liezelSatisfiedHaSFX;
    [SerializeField] private AudioClip liezelShockGaspSFX;
    [SerializeField] private AudioClip liezelThinkingSFX;
    [SerializeField] private AudioClip liezelWonderingSFX;

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
            // Santi
            case "<SFX_santi_concerned>": sfxSource.PlayOneShot(santiConcernedSFX); break;
            case "<SFX_santi_frustrated>": sfxSource.PlayOneShot(santiFrustratedSFX); break;
            case "<SFX_santi_happy>": sfxSource.PlayOneShot(santiHappySFX); break;
            case "<SFX_santi_oh>": sfxSource.PlayOneShot(santiOhSFX); break;
            case "<SFX_santi_questioning>": sfxSource.PlayOneShot(santiQuestioningSFX); break;
            case "<SFX_santi_relieved>": sfxSource.PlayOneShot(santiRelievedSFX); break;
            case "<SFX_santi_satisfied>": sfxSource.PlayOneShot(santiSatisfiedSFX); break;
            case "<SFX_santi_shock_gasp>": sfxSource.PlayOneShot(santiShockGaspSFX); break;
            case "<SFX_santi_sigh>": sfxSource.PlayOneShot(santiSighSFX); break;
            case "<SFX_santi_thinking>": sfxSource.PlayOneShot(santiThinkingSFX); break;
            case "<SFX_santi_wondering>": sfxSource.PlayOneShot(santiWonderingSFX); break;

            // Liezel
            case "<SFX_liezel_concerned>": sfxSource.PlayOneShot(liezelConcernedSFX); break;
            case "<SFX_liezel_discovery>": sfxSource.PlayOneShot(liezelDiscoverySFX); break;
            case "<SFX_liezel_frustrated>": sfxSource.PlayOneShot(liezelFrustratedSFX); break;
            case "<SFX_liezel_happy>": sfxSource.PlayOneShot(liezelHappySFX); break;
            case "<SFX_liezel_oh>": sfxSource.PlayOneShot(liezelOhSFX); break;
            case "<SFX_liezel_questioning>": sfxSource.PlayOneShot(liezelQuestioningSFX); break;
            case "<SFX_liezel_relieved>": sfxSource.PlayOneShot(liezelRelievedSFX); break;
            case "<SFX_liezel_satisfied_(aha!)>": sfxSource.PlayOneShot(liezelSatisfiedAhaSFX); break;
            case "<SFX_liezel_satisfied_(ha!)>": sfxSource.PlayOneShot(liezelSatisfiedHaSFX); break;
            case "<SFX_liezel_shock_gasp>": sfxSource.PlayOneShot(liezelShockGaspSFX); break;
            case "<SFX_liezel_thinking>": sfxSource.PlayOneShot(liezelThinkingSFX); break;
            case "<SFX_liezel_wondering>": sfxSource.PlayOneShot(liezelWonderingSFX); break;

            default: break;
        }
    }
}
