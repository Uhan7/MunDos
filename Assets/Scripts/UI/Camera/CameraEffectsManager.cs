using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameObject mainCamera;

    [Header("Other Variables")]
    [SerializeField] private AudioClip shakeSFX;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) ShakeScreen();
    }

    // Helper Functions --------------------------------------------------------

    public void ShakeScreen()
    {
        mainCamera.GetComponent<CameraShake>().ScreenShakeWrapper();
        sfxSource.PlayOneShot(shakeSFX);
    }
}
