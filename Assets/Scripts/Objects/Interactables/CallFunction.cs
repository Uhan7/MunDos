using UnityEngine;
using UnityEngine.Events;

public class CallFunction : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("References")]
    [SerializeField] public GameObject[] itemsToAdd;
    [SerializeField] public UnityEvent callFunc;

    [Header("Properties")]
    [SerializeField] private bool onEnable;
    [SerializeField] private bool onTrigger;
    [SerializeField] private bool disableAfter;

    private void Start()
    {
        if (callFunc == null) Debug.Log($"{this.name}'s callFunc is empty");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (onTrigger)
        {
            callFunc.Invoke();
        }
        if (disableAfter) this.enabled = false;
    }
    private void OnEnable()
    {
        if (onEnable)
        {
            callFunc.Invoke();
        }
        if (disableAfter) this.enabled = false;
    }
}
