using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class camera_shake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private bool shakeOnEnable;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private AnimationCurve strengthCurve;
    [SerializeField] private float strengthMultiplier = 1f;
    [SerializeField] private bool dontResetToZero;

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

    private IEnumerator screenShake()
    {
        // 1) Find the CinemachineBrain on the main camera
        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("camera_shake: No CinemachineBrain found on Camera.main");
            yield break;
        }

        // 2) Get its ActiveVirtualCamera (an ICinemachineCamera)
        var activeCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (activeCam == null)
        {
            Debug.LogError("camera_shake: ActiveVirtualCamera is not a CinemachineVirtualCamera");
            yield break;
        }

        // 3) Grab the Perlin noise component
        var perlin = activeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogError("camera_shake: No BasicMultiChannelPerlin on active vcam");
            yield break;
        }

        // 4) Shake over time
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

        // 5) Reset to zero so it stops
        if (!dontResetToZero)
        {
            perlin.FrequencyGain = 0f;
            perlin.AmplitudeGain = 0f;
        }
    }
}
