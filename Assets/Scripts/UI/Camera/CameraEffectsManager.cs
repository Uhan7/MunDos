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

    [Header("Voices SFX: Antiquarian")]
    [SerializeField] private AudioClip antiquarianFrustratedGruntSFX;
    [SerializeField] private AudioClip antiquarianNormalSuspiciousSFX;
    [SerializeField] private AudioClip antiquarianOminousChuckle1SFX;
    [SerializeField] private AudioClip antiquarianOminousChuckle2SFX;
    [SerializeField] private AudioClip antiquarianProudSFX;
    [SerializeField] private AudioClip antiquarianSatisfiedSFX;
    [SerializeField] private AudioClip antiquarianSnickerSFX;
    [SerializeField] private AudioClip antiquarianSurprisedGaspSFX;
    [SerializeField] private AudioClip antiquarianThinkingSFX;

    [Header("Voices SFX: Ate Guard")]
    [SerializeField] private AudioClip ateGuardNormalSFX;
    [SerializeField] private AudioClip ateGuardProudSFX;
    [SerializeField] private AudioClip ateGuardSigh2SFX;
    [SerializeField] private AudioClip ateGuardSighSFX;
    [SerializeField] private AudioClip ateGuardSurprisedGaspSFX;
    [SerializeField] private AudioClip ateGuardThinkingSFX;

    [Header("Voices SFX: Distressed Woman")]
    [SerializeField] private AudioClip distressedWomanAnnoyedSFX;
    [SerializeField] private AudioClip distressedWomanCrying1SFX;
    [SerializeField] private AudioClip distressedWomanCrying2SFX;
    [SerializeField] private AudioClip distressedWomanDisgustSFX;
    [SerializeField] private AudioClip distressedWomanFrustratedSFX;
    [SerializeField] private AudioClip distressedWomanNormalSFX;
    [SerializeField] private AudioClip distressedWomanSighSFX;
    [SerializeField] private AudioClip distressedWomanSurprisedGaspSFX;

    [Header("Voices SFX: Manong Guard")]
    [SerializeField] private AudioClip manongGuardFrustratedSFX;
    [SerializeField] private AudioClip manongGuardNormalSFX;
    [SerializeField] private AudioClip manongGuardProudSFX;
    [SerializeField] private AudioClip manongGuardSighSFX;
    [SerializeField] private AudioClip manongGuardThinkingSFX;

    [Header("Voices SFX: Pare Guard")]
    [SerializeField] private AudioClip pareGuardFrustratedSighSFX;
    [SerializeField] private AudioClip pareGuardFrustratedUghSFX;
    [SerializeField] private AudioClip pareGuardProudSFX;
    [SerializeField] private AudioClip pareGuardSatisfied1SFX;
    [SerializeField] private AudioClip pareGuardSatisfied2SFX;
    [SerializeField] private AudioClip pareGuardSatisfiedMhm3SFX;
    [SerializeField] private AudioClip pareGuardSurprised1SFX;
    [SerializeField] private AudioClip pareGuardSurprised2SFX;
    [SerializeField] private AudioClip pareGuardThinkingSFX;

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

        flashObject.GetComponent<Fader>().FadeSequence(0.6f, 0.05f, 0, 0.45f);
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

        dimObject.GetComponent<Fader>().FadeSequence(0.6f, 0.05f, 0, 0.45f);
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

            // Antiquarian
            case "<SFX_antiquarian_frustrated_grunt>": sfxSource.PlayOneShot(antiquarianFrustratedGruntSFX); break;
            case "<SFX_antiquarian_normal_suspicious>": sfxSource.PlayOneShot(antiquarianNormalSuspiciousSFX); break;
            case "<SFX_antiquarian_ominous_chuckle_1>": sfxSource.PlayOneShot(antiquarianOminousChuckle1SFX); break;
            case "<SFX_antiquarian_ominous_chuckle_2>": sfxSource.PlayOneShot(antiquarianOminousChuckle2SFX); break;
            case "<SFX_antiquarian_proud>": sfxSource.PlayOneShot(antiquarianProudSFX); break;
            case "<SFX_antiquarian_satisfied>": sfxSource.PlayOneShot(antiquarianSatisfiedSFX); break;
            case "<SFX_antiquarian_snicker>": sfxSource.PlayOneShot(antiquarianSnickerSFX); break;
            case "<SFX_antiquarian_surprised_gasp>": sfxSource.PlayOneShot(antiquarianSurprisedGaspSFX); break;
            case "<SFX_antiquarian_thinking>": sfxSource.PlayOneShot(antiquarianThinkingSFX); break;

            // Ate Guard
            case "<SFX_ate_guard_normal>": sfxSource.PlayOneShot(ateGuardNormalSFX); break;
            case "<SFX_ate_guard_proud>": sfxSource.PlayOneShot(ateGuardProudSFX); break;
            case "<SFX_ate_guard_sigh_2>": sfxSource.PlayOneShot(ateGuardSigh2SFX); break;
            case "<SFX_ate_guard_sigh>": sfxSource.PlayOneShot(ateGuardSighSFX); break;
            case "<SFX_ate_guard_surprised_gasp>": sfxSource.PlayOneShot(ateGuardSurprisedGaspSFX); break;
            case "<SFX_ate_guard_thinking>": sfxSource.PlayOneShot(ateGuardThinkingSFX); break;

            // Distressed Woman
            case "<SFX_distressed_woman_annoyed>": sfxSource.PlayOneShot(distressedWomanAnnoyedSFX); break;
            case "<SFX_distressed_woman_crying_1>": sfxSource.PlayOneShot(distressedWomanCrying1SFX); break;
            case "<SFX_distressed_woman_crying_2>": sfxSource.PlayOneShot(distressedWomanCrying2SFX); break;
            case "<SFX_distressed_woman_disgust>": sfxSource.PlayOneShot(distressedWomanDisgustSFX); break;
            case "<SFX_distressed_woman_frustrated>": sfxSource.PlayOneShot(distressedWomanFrustratedSFX); break;
            case "<SFX_distressed_woman_normal>": sfxSource.PlayOneShot(distressedWomanNormalSFX); break;
            case "<SFX_distressed_woman_sigh>": sfxSource.PlayOneShot(distressedWomanSighSFX); break;
            case "<SFX_distressed_woman_surprised_gasp>": sfxSource.PlayOneShot(distressedWomanSurprisedGaspSFX); break;

            // Manong Guard
            case "<SFX_manong_guard_frustrated>": sfxSource.PlayOneShot(manongGuardFrustratedSFX); break;
            case "<SFX_manong_guard_normal>": sfxSource.PlayOneShot(manongGuardNormalSFX); break;
            case "<SFX_manong_guard_proud>": sfxSource.PlayOneShot(manongGuardProudSFX); break;
            case "<SFX_manong_guard_sigh>": sfxSource.PlayOneShot(manongGuardSighSFX); break;
            case "<SFX_manong_guard_thinking>": sfxSource.PlayOneShot(manongGuardThinkingSFX); break;

            // Pare Guard
            case "<SFX_pare_guard_frustrated_sigh>": sfxSource.PlayOneShot(pareGuardFrustratedSighSFX); break;
            case "<SFX_pare_guard_frustrated_ugh>": sfxSource.PlayOneShot(pareGuardFrustratedUghSFX); break;
            case "<SFX_pare_guard_proud>": sfxSource.PlayOneShot(pareGuardProudSFX); break;
            case "<SFX_pare_guard_satisfied_1>": sfxSource.PlayOneShot(pareGuardSatisfied1SFX); break;
            case "<SFX_pare_guard_satisfied_2>": sfxSource.PlayOneShot(pareGuardSatisfied2SFX); break;
            case "<SFX_pare_guard_satisfied_mhm_3>": sfxSource.PlayOneShot(pareGuardSatisfiedMhm3SFX); break;
            case "<SFX_pare_guard_surprised_1>": sfxSource.PlayOneShot(pareGuardSurprised1SFX); break;
            case "<SFX_pare_guard_surprised_2>": sfxSource.PlayOneShot(pareGuardSurprised2SFX); break;
            case "<SFX_pare_guard_thinking>": sfxSource.PlayOneShot(pareGuardThinkingSFX); break;

            default: break;
        }
    }
}
