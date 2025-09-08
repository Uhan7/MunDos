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

    public void Flash()
    {
        flashObject.SetActive(false);
        flashObject.SetActive(true);
        sfxSource.PlayOneShot(flashSFX);
    }

    public void Dim()
    {
        dimObject.SetActive(false);
        dimObject.SetActive(true);
        sfxSource.PlayOneShot(dimSFX);
    }
}
