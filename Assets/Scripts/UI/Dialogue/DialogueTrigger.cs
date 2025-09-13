using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const float DEACTIVATE_TIME = 0.1f;
    [HideInInspector] private const string PROTAG_TAG = "Protag";

    [Header("References")]
    [SerializeField] private string dialogueHolderName = "Dialogue Holder";
    [HideInInspector] private DialogueManager dialogueHolder;
    [SerializeField] private Dialogue dialogue;
    [HideInInspector] private GameObject protag;
    [HideInInspector] private PlayerMove protagMovementScript;

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
    [SerializeField] private bool startOnEnableWithDelay;
    [SerializeField] private bool startFromTrigger;
    [SerializeField] private bool isSign;
    [SerializeField] private bool willFocus;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private bool linksToOtherDialogue;
    [SerializeField] private AnimOptions playClosingAnimation = AnimOptions.defaultClose;
    [SerializeField] private GameObject[] objectsToSpawnAfter;
    [SerializeField] private float activateObjectTime = 0.5f;
    [ShowIf("linksToOtherDialogue")] [SerializeField] private GameObject nextDialogue;
    [ShowIf("linksToOtherDialogue")] [SerializeField] private float nextDialogueTime = 0.5f;

    [Header("Timers")]
    [HideInInspector] private float deactivateTimer;
    [HideInInspector] private float nextDialogueTimer;
    [HideInInspector] private float activateObjectTimer;

    [Header("Flags")]
    private bool dialogueIsTriggered;

    

    private void Awake()
    {
        InitializeReferences();
    }

    private void Start()
    {
        activateObjectTimer = activateObjectTime;
    }

    private void OnEnable()
    {
        ResetValues();

        if (startOnEnable) TriggerDialogue();
    }

    private void Update()
    {
        //if (objectsToSpawnAfter.Length != 0)
        //{
        //    WaitForOtherDialogue();
        //    if (activateObjectTimer <= 0) ActivateOtherObjects();
        //}

        if (dialogueHolder == null || dialogueHolder.open || !dialogueIsTriggered) return;

        if (objectsToSpawnAfter.Length != 0)
        {
            Debug.Log("test 1");
            //WaitForOtherDialogue();
            //if (activateObjectTimer <= 0)
                ActivateOtherObjects();
        }

        if (nextDialogue != null)
        {
            WaitForOtherDialogue();

            if (nextDialogueTimer <= 0)
            {
                ActivateOtherDialogue(nextDialogue);
            }
        }

        else if (deactivateAfter)
        {
            dialogueHolder.EndDialogue();
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!startFromTrigger) return;

        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            TriggerDialogue();
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (!startFromTrigger) return;

        if (col.gameObject.CompareTag(PROTAG_TAG))
        {
            if (isSign) dialogueHolder.EndDialogue();
            if (isSign && deactivateAfter) Deactivate();
        }
    }

    // Helper Functions --------------------------------------------------------

    public void TriggerDialogue()
	{
        if (playClosingAnimation == AnimOptions.defaultClose && linksToOtherDialogue)
        {
            closeAnim = false;
        } else if (playClosingAnimation == AnimOptions.skipClose) {
            closeAnim = false;
        }

        dialogueHolder.playClosingAnimation = closeAnim;

        if (dialogueHolder == null || !dialogueHolder.gameObject.activeInHierarchy) dialogueHolder = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
        if (dialogueHolder == null || !dialogueHolder.gameObject.activeInHierarchy) Debug.LogError("Dialogue Holder not Found.");

        if (willFocus) Focus(true);

        dialogueIsTriggered = true;
        if (startFromTrigger && !isSign) GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(dialogueHolder.StartDialogue(dialogue));
    }

    void WaitForOtherDialogue()
    {
        dialogueHolder.EndDialogue();
        nextDialogueTimer -= Time.deltaTime;
        activateObjectTimer -= Time.deltaTime;
    }

    public void ActivateOtherDialogue(GameObject nextDialogue)
    {
        nextDialogue.transform.position = GameObject.FindGameObjectWithTag(PROTAG_TAG).transform.position;
        nextDialogue.SetActive(true);

        if (deactivateAfter) gameObject.SetActive(false);
    }

    void ActivateOtherObjects()
    {
        foreach (GameObject objs in objectsToSpawnAfter) objs.SetActive(true);
    }

    public void Deactivate()
    {
        protag = null;
        if (willFocus) Focus(false);

        deactivateTimer -= Time.deltaTime;
        if (deactivateTimer <= 0) gameObject.SetActive(false);
    }

    public void ResetValues()
    {
        protag = null;

        deactivateTimer = DEACTIVATE_TIME;
        nextDialogueTimer = nextDialogueTime;

        dialogueIsTriggered = false;
    }

    void InitializeReferences()
    {
        dialogueHolder = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }
}
