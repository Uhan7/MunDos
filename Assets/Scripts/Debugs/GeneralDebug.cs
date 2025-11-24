using UnityEngine;

public class GeneralDebug : MonoBehaviour
{
    [SerializeField] private bool debugWhenEnabled;

    void OnEnable()
    {
        if (debugWhenEnabled) print(gameObject.name + " called Debug When Enabled.");
    }
}
