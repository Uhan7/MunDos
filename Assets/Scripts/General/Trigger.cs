using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Trigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const float DEACTIVATE_TIME = 0.1f;
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Properties")]
    [ShowIf("hasTimer")][SerializeField] private bool willFocus;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private bool hasTimer = true;
    [SerializeField] private GameObject[] activateObjects;
    [SerializeField] private GameObject[] deactivateObjects;
    [HideInInspector] private bool hasActivatedObejcts = false;
    [HideInInspector] private bool isTriggered = false;
    [HideInInspector] private bool finishTrigger = false;

    [Header("Timers")]
    [ShowIf("hasTimer")][SerializeField] private float timeToWait = 1.5f;
    [ShowIf("hasTimer")][SerializeField] private float objectActiveStateAtTime = 1.0f;
    [ShowIf("hasTimer")][HideInInspector] private float countdownTimer;
    [ShowIf("hasTimer")][HideInInspector] private float delay = 0.5f;

    private void Start()
    {
        countdownTimer = timeToWait;
    }

    private void OnEnable()
    {
        ResetValues();
    }
    private void Update()
    {
        if (finishTrigger) return;
        if (hasTimer && isTriggered)
        {
            EnableTrigger();
        }
        else if (!hasTimer && isTriggered)
        {
            SetObjectStates();
            finishTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            isTriggered = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            if (deactivateAfter) Deactivate();
            ResetValues();
        }
    }

    // Helper Functions --------------------------------------------------------

    void WaitForTimer()
    {
        countdownTimer -= Time.deltaTime;
        //Debug.Log("timer: " + countdownTimer);
    }
    public void EnableTrigger()
	{
        WaitForTimer();
        if (willFocus) Focus(true);
        if(countdownTimer <= objectActiveStateAtTime && !hasActivatedObejcts)
        {
            SetObjectStates();
        }

        if (countdownTimer <= 0)
        {
            Focus(false);
            finishTrigger = true;
        }
    }

    private void SetObjectStates()
    {
        if (activateObjects.Length != 0) foreach (GameObject objs in activateObjects) objs.SetActive(true);
        if (deactivateObjects.Length != 0) foreach (GameObject objs in deactivateObjects) objs.SetActive(false);
        hasActivatedObejcts = true;
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
