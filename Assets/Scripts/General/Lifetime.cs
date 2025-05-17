using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private bool destroy;
    [SerializeField] private bool deactivate;

    [SerializeField] private float lifetime;

    private void OnEnable()
    {
        if (destroy) Destroy(gameObject, lifetime);
        if (deactivate) gameObject.SetActive(false);
    }

}
