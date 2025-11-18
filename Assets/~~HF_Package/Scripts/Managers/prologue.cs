using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class prologue : MonoBehaviour
{
    public AudioSource aSource;
    public GameObject skippingText;
    public Image skipMeter;
    [SerializeField] private string titleScreenSceneName;

    private float skipHoldTime = 3f;
    private float escapeHeldDuration = 0f;
    private bool hasSkipped = false;

    private float originalVolume = 0.75f;
    private float fadeSpeed = 3f;

    void Awake()
    {
        Time.timeScale = 0;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (skipMeter != null)
        {
            skipMeter.fillAmount = 0;
            skipMeter.gameObject.SetActive(false);
        }

        if (aSource != null)
        {
            aSource.volume = originalVolume;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(StartPrologue());
    }

    private void Update()
    {
        if (hasSkipped) return;

        bool holdingEscape = Input.GetKey(KeyCode.Escape);

        if (holdingEscape)
        {
            skippingText.SetActive(true);
            escapeHeldDuration += Time.unscaledDeltaTime;

            if (skipMeter != null)
            {
                skipMeter.gameObject.SetActive(true);
                skipMeter.fillAmount = escapeHeldDuration / skipHoldTime;
            }

            if (escapeHeldDuration >= skipHoldTime)
            {
                hasSkipped = true;
                StartGame();
            }
        }
        else
        {
            skippingText.SetActive(false);

            if (escapeHeldDuration > 0f)
            {
                escapeHeldDuration = 0f;

                if (skipMeter != null)
                {
                    skipMeter.fillAmount = 0f;
                    skipMeter.gameObject.SetActive(false);
                }
            }
        }

        // 🔉 Smoothly interpolate volume
        if (aSource != null)
        {
            float targetVolume = holdingEscape ? 0f : originalVolume;
            aSource.volume = Mathf.Lerp(aSource.volume, targetVolume, Time.unscaledDeltaTime * fadeSpeed);
        }
    }

    IEnumerator StartPrologue()
    {
        yield return new WaitUntil(() => aSource != null && aSource.clip != null);
        yield return null;

        Time.timeScale = 1;
        aSource.Play();
    }

    void StartGame()
    {
        SceneManager.LoadScene(titleScreenSceneName);
    }
}
