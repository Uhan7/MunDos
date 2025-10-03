using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Fader : MonoBehaviour
{
    [Header("Possible Components")]
    [HideInInspector] private Image imageComponent;
    [HideInInspector] private TextMeshProUGUI textComponent;
    [HideInInspector] private SpriteRenderer spriteRendererComponent;

    void Awake()
    {
        if (GetComponent<Image>() != null) imageComponent = GetComponent<Image>();
        if (GetComponent<TextMeshProUGUI>() != null) textComponent = GetComponent<TextMeshProUGUI>();
        if (GetComponent<SpriteRenderer>() != null) spriteRendererComponent = GetComponent<SpriteRenderer>();
    }

    public void FadeToWrapper(float targetAlpha, float duration)
    {
        StartCoroutine(FadeTo(targetAlpha, duration));
    }

    public IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float time = 0f;

        Color startColor = Color.white;
        if (imageComponent != null) startColor = imageComponent.color;
        else if (textComponent != null) startColor = textComponent.color;
        else if (spriteRendererComponent != null) startColor = spriteRendererComponent.color;

        float startAlpha = startColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            Color newColor = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            if (imageComponent != null) imageComponent.color = newColor;
            if (textComponent != null) textComponent.color = newColor;
            if (spriteRendererComponent != null) spriteRendererComponent.color = newColor;

            yield return null;
        }
    }
}
