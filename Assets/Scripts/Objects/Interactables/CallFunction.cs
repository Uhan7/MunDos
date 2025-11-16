using NaughtyAttributes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class CallFunction : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("References")]

    [SerializeField] private bool activatedByKeyPress;
    [ShowIf("activatedByKeyPress")][SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] public UnityEvent toCall;
    [ShowIf("activatedByKeyPress")][SerializeField] private bool activateBeforeKeyPress;
    [ShowIf("activateBeforeKeyPress")][SerializeField] public UnityEvent toCallRegardless;


    [Header("Properties")]
    [SerializeField] private bool onEnable = true;
    [SerializeField] private bool onTrigger;
    [SerializeField] private bool disableAfter = true;
    [HideInInspector] private bool hasInvoked;

    private void Start()
    {
        if (toCall == null) Debug.Log($"{this.name}'s callFunc is empty");
        hasInvoked = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (onTrigger)
        {
            if (activatedByKeyPress)
            {
                CheckKeyPress();
            }
            else if (!activatedByKeyPress) toCall.Invoke();
            if (disableAfter)
            {
                if (activatedByKeyPress && hasInvoked) Disable();
                else if (!activatedByKeyPress) Disable();

            }
        }
    }

    private void OnEnable()
    {
        Debug.Log($"CF: isactive");
        if (onEnable) 
        {
            Debug.Log($"CF: invoking");
            if (activatedByKeyPress) 
            {
                CheckKeyPress();
            }
            else if (!activatedByKeyPress) toCall.Invoke();
            if (disableAfter)
            {
                if (activatedByKeyPress && hasInvoked) Disable();
                else if (!activatedByKeyPress) Disable();
            }
        }
    }

    private void CheckKeyPress()
    {
        if (toCallRegardless != null) toCallRegardless.Invoke();
        if (Input.GetKeyDown(interactKey))
        {
            toCall.Invoke();
            hasInvoked = true;
        }
    }
    private void Disable()
    {
        this.gameObject.SetActive(false);
        hasInvoked = false;
    }
}
