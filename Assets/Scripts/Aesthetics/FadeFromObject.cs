using UnityEngine;

public class FadeFromObject : MonoBehaviour
{
    [Header("Components")]
    private SpriteRenderer spriteRenderer;

    [Header("Variables")]
    [SerializeField] private Transform targetSource;
    [SerializeField] private float minimumDistance = 1f;
    [SerializeField] private float maximumDistance = 5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetSource == null) return;

        float distance = Vector2.Distance(transform.position, targetSource.position);
        if (distance > maximumDistance) return;

        float alpha = 1f - Mathf.InverseLerp(minimumDistance, maximumDistance, distance);

        UpdateAlpha(alpha);
    }

    void UpdateAlpha(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
}