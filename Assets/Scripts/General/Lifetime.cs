using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private bool destroy;
    [SerializeField] private bool deactivate;

    [SerializeField] private float lifeTime;
    [HideInInspector] private float lifeTimer;

    private void OnEnable()
    {
        if (destroy) Destroy(gameObject, lifeTime);
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (deactivate && lifeTime < 0) gameObject.SetActive(false);
    }

}
