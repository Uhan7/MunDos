using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    private Image image;

    [SerializeField] private float fadeInDuration = 1.5f;  // Time to fade in
    [SerializeField] private float fadeOutDelay = 3f;      // Time before fading out
    [SerializeField] private float fadeOutDuration = 1.5f; // Time to fade out
    [SerializeField] private bool disableOnFadeOut = true; // Whether to disable after fade-out

    private void Awake()
    {
        image = GetComponent<Image>();

        // Start with a transparent image
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    private void OnEnable()
    {
        // Start fade-in when the object is activated
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Step 1: Fade in
        yield return StartCoroutine(FadeImage(0f, 1f, fadeInDuration));

        // Step 2: Wait before fading out
        yield return new WaitForSeconds(fadeOutDelay);

        // Step 3: Fade out
        yield return StartCoroutine(FadeImage(1f, 0f, fadeOutDuration));

        // Step 4: Optionally disable after fade-out
        if (disableOnFadeOut)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure final alpha is exactly set
        image.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
