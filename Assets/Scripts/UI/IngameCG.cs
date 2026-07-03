using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngameCG : MonoBehaviour
{
    private Image image;

    [SerializeField] private float delayTime;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool disableOnFadeOut = true;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (fadeIn) StartCoroutine(FadeInSequence());
    }

    public void FadeOutWrapper()
    {
        StartCoroutine(FadeOutSequence());
    }

    private IEnumerator FadeInSequence()
    {
        yield return StartCoroutine(FadeImage(0f, 1f, delayTime));
    }

    private IEnumerator FadeOutSequence()
    {
        yield return StartCoroutine(FadeImage(1f, 0f, delayTime));
        if (disableOnFadeOut) gameObject.SetActive(false);
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
        // image.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
