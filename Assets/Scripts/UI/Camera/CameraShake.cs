using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private bool shakeOnEnable;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private AnimationCurve strengthCurve;
    [SerializeField] private float strengthMultiplier = 1f;

    private void OnEnable()
    {
        if (shakeOnEnable)
            StartCoroutine(screenShake());
    }

    /// <summary>
    /// Call this to trigger a shake on whatever vcam is live.
    /// </summary>
    public void ScreenShakeWrapper()
    {
        StartCoroutine(screenShake());
    }

    public void StartScreenShakeWrapper()
    {
        StartCoroutine(startScreenShake());
    }

    public void ScreenShakeWrapper(float shakeDuration)
    {
        StartCoroutine(screenShake(shakeDuration));
    }

    public void EndScreenShakeWrapper()
    {
        StartCoroutine(endScreenShake());
    }

    private IEnumerator startScreenShake()
    {
        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("camera_shake: No CinemachineBrain found on Camera.main");
            yield break;
        }

        var activeCam = brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCam == null)
        {
            Debug.LogError("camera_shake: ActiveVirtualCamera is not a CinemachineVirtualCamera");
            yield break;
        }

        var perlin = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogError("camera_shake: No BasicMultiChannelPerlin on active vcam");
            yield break;
        }

        perlin.FrequencyGain = strengthMultiplier;
        perlin.AmplitudeGain = strengthMultiplier * 2f;
    }

    private IEnumerator screenShake()
    {
        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("camera_shake: No CinemachineBrain found on Camera.main");
            yield break;
        }

        var activeCam = brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCam == null)
        {
            Debug.LogError("camera_shake: ActiveVirtualCamera is not a CinemachineVirtualCamera");
            yield break;
        }

        var perlin = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogError("camera_shake: No BasicMultiChannelPerlin on active vcam");
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float t = elapsed / shakeDuration;
            float strength = strengthCurve.Evaluate(t) * strengthMultiplier;
            perlin.FrequencyGain = strength;
            perlin.AmplitudeGain = strength * 2f;

            elapsed += Time.deltaTime;
            yield return null;
        }

        perlin.FrequencyGain = 0f;
        perlin.AmplitudeGain = 0f;
    }

    private IEnumerator screenShake(float shakeDuration)
    {
        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("camera_shake: No CinemachineBrain found on Camera.main");
            yield break;
        }

        var activeCam = brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCam == null)
        {
            Debug.LogError("camera_shake: ActiveVirtualCamera is not a CinemachineVirtualCamera");
            yield break;
        }

        var perlin = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogError("camera_shake: No BasicMultiChannelPerlin on active vcam");
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float t = elapsed / shakeDuration;
            float strength = strengthCurve.Evaluate(t) * strengthMultiplier;
            perlin.FrequencyGain = strength;
            perlin.AmplitudeGain = strength * 2f;

            elapsed += Time.deltaTime;
            yield return null;
        }

        perlin.FrequencyGain = 0f;
        perlin.AmplitudeGain = 0f;
    }

    private IEnumerator endScreenShake()
    {
        StopAllCoroutines();

        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("camera_shake: No CinemachineBrain found on Camera.main");
            yield break;
        }

        var activeCam = brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCam == null)
        {
            Debug.LogError("camera_shake: ActiveVirtualCamera is not a CinemachineVirtualCamera");
            yield break;
        }

        var perlin = activeCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogError("camera_shake: No BasicMultiChannelPerlin on active vcam");
            yield break;
        }

        perlin.FrequencyGain = 0f;
        perlin.AmplitudeGain = 0f;

    }

}
