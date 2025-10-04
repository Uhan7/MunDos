using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Trigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const float DEACTIVATE_TIME = 0.1f;
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("Animation Properties")]
    private bool closeAnim = true;
    enum AnimOptions
    {
        defaultClose = 0,
        playClose,
        skipClose
    };

    [Header("Properties")]
    [SerializeField] private bool startOnEnable;
    [SerializeField] private bool startFromTrigger = true;
    [SerializeField] private bool willFocus;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private GameObject[] activateObjects;
    [SerializeField] private GameObject[] deactivateObjects;
    [SerializeField] private bool isTriggered = false;

    [Header("Timers")]
    [SerializeField] private float HoldTime = 1.5f;
    [SerializeField] private float objectActiveTime = 1.0f;
    [HideInInspector] private float countdownTimer;

    private void Start()
    {
        countdownTimer = HoldTime;
    }

    private void OnEnable()
    {
        ResetValues();

        if (startOnEnable) EnableTrigger();
    }

    private void Update()
    {
        if (isTriggered)
        {
            EnableTrigger();
        }
        

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!startFromTrigger) return;

        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            isTriggered = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (!startFromTrigger) return;

        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            if (deactivateAfter) Deactivate();
        }
    }

    // Helper Functions --------------------------------------------------------

    void WaitForTimer()
    {
        countdownTimer -= Time.deltaTime;
    }
    public void EnableTrigger()
	{
        WaitForTimer();
        if (willFocus) Focus(true);
        if (activateObjects.Length != 0 && countdownTimer <= objectActiveTime)
        {
            ActivateOtherObjects();
        }
        if (deactivateObjects.Length != 0 && countdownTimer <= objectActiveTime)
        {
            DeactivateOtherObjects();
        }

        if (countdownTimer <= 0)
        {
            Focus(false);
        }

    }
    void ActivateOtherObjects()
    {
        Debug.Log("Activating Objects");
        foreach (GameObject objs in activateObjects) objs.SetActive(true);
    }
    void DeactivateOtherObjects()
    {
        Debug.Log("Deactivating Objects");
        foreach (GameObject objs in deactivateObjects) objs.SetActive(false);
    }

    public void Deactivate()
    {
        if (willFocus)
        {
            Focus(false);
        }
        if (countdownTimer <= 0) gameObject.SetActive(false);
    }

    public void ResetValues()
    {
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
}
