using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject flashObject;
    [SerializeField] private GameObject dimObject;

    [Header("Other Variables")]
    [SerializeField] private AudioClip shakeSFX;
    [SerializeField] private AudioClip flashSFX;
    [SerializeField] private AudioClip dimSFX;

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
        flashObject.GetComponent<Animator>().Play("image_fade_out_half");
        sfxSource.PlayOneShot(flashSFX);
    }

    public void StartFlash()
    {
        //flashObject.GetComponent<Animator>().Play("image_fade_in_half");
        flashObject.GetComponent<Fader>().FadeToWrapper(1, 1);
        sfxSource.PlayOneShot(flashSFX);
    }

    public void EndFlash()
    {
        //flashObject.GetComponent<Animator>().Play("image_fade_out_half");
        flashObject.GetComponent<Fader>().FadeToWrapper(0, 1);
    }

    public void Dim()
    {
        dimObject.GetComponent<Animator>().Play("image_fade_out_half");
        sfxSource.PlayOneShot(dimSFX);
    }

    public void StartDim()
    {
        //dimObject.GetComponent<Animator>().Play("image_fade_in_half");
        dimObject.GetComponent<Fader>().FadeToWrapper(1, 1);
        sfxSource.PlayOneShot(dimSFX);
    }

    public void EndDim()
    {
        //dimObject.GetComponent<Animator>().Play("image_fade_out_half");
        dimObject.GetComponent<Fader>().FadeToWrapper(0, 1);
    }
}
