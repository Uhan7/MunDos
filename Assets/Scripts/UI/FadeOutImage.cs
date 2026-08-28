using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    [HideInInspector] private Image image;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        image = GetComponent<Image>();

        image.CrossFadeAlpha(0.0f, fadeDuration, false);
    }
}
