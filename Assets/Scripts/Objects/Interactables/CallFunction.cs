using NaughtyAttributes;
using System.Collections;
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
    [SerializeField] private bool willFocus = true;
    [HideInInspector] private bool hasInvoked;

    private void Start()
    {
        hasInvoked = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        if (onTrigger)
        {
            if (willFocus)
            {
                Focus(true);
            }
            
            if (activatedByKeyPress)
            {
                CheckKeyPress();
            }
            else if (!activatedByKeyPress) toCall.Invoke();
        }
        if (disableAfter)
        {
            if (activatedByKeyPress && hasInvoked) Disable();
            else if (!activatedByKeyPress) Disable();

        }
    }

    private void OnEnable()
    {
        if (onEnable) 
        {
            if (willFocus)
            {
                Focus(true);
            }
            if (activatedByKeyPress) 
            {
                CheckKeyPress();
            }
            else if (!activatedByKeyPress) toCall.Invoke();
        }
        if (disableAfter)
        {
            if (activatedByKeyPress && hasInvoked)
            {
                Disable();
            }
            else if (!activatedByKeyPress) Disable();
        }
    }
     
    private void CheckKeyPress()
    {
        if (toCallRegardless != null) toCallRegardless.Invoke();
        StartCoroutine(WaitForKeyPress());
    }

    IEnumerator WaitForKeyPress()
    {
        Focus(true);
        yield return new WaitForSeconds(0.2f);
        while (!Input.GetKeyDown(interactKey))
        {
            yield return null;
        }
        hasInvoked = true;
        toCall.Invoke();
        Disable();
    }

    private void Disable()
    {
        Focus(false);
        hasInvoked = false;
        this.gameObject.SetActive(false);
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
}
