using UnityEngine;

public class LightReaction : MonoBehaviour
{
    [SerializeField] Renderer objectRenderer;
    [HideInInspector] Material objectMaterial;

    [SerializeField] Color emissionColor = Color.white;
    [SerializeField] float lightIntensity = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectMaterial = objectRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        objectMaterial.SetColor("_EmissionColor", emissionColor * Mathf.PingPong(Time.time * lightIntensity, 1f));
    }
}
