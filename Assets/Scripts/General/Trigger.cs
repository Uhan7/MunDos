using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Trigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Properties")]
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool deactivateAfter;
    [SerializeField] private bool willFocus;
    [HideInInspector] private bool isTriggered = false;
    [HideInInspector] private bool finishTrigger = false;

    [Header("GameObjects Reference")]
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeactivate;

    [Header("Timers")]
    [SerializeField] private float waitforSeconds = 0.0f;

    [Header("Flags")]
    [HideInInspector] private bool hasActivatedObejcts = false;

    private void OnEnable()
    {
        if (isOnEnable)
        {
            if (willFocus) Focus(true);
            if (objectsToActivate.Length > 0) StartCoroutine(ActivateAfterTime(waitforSeconds));
            if (objectsToDeactivate.Length > 0) StartCoroutine(DeactivateAfterTime(waitforSeconds));
            if (willFocus) Focus(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;
        
        if (willFocus) Focus(true);
        if (objectsToActivate.Length > 0) StartCoroutine(ActivateAfterTime(waitforSeconds));
        if (objectsToDeactivate.Length > 0) StartCoroutine(DeactivateAfterTime(waitforSeconds));
        if (willFocus) StartCoroutine(Wait(waitforSeconds));
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(PROTAG_TAG)) return;

        ResetValues();
        if (deactivateAfter) Deactivate();
    }

    // Helper Functions --------------------------------------------------------

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToActivate) obj.SetActive(true);
    }

    IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var obj in objectsToDeactivate) obj.SetActive(false);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Focus(false);
    }

    public void Deactivate()
    {
        if (willFocus)
        {
            Focus(false);
        }
        gameObject.SetActive(false);
    }

    public void ResetValues()
    {
        hasActivatedObejcts = false;
        isTriggered = false;
        finishTrigger = false;
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
}
