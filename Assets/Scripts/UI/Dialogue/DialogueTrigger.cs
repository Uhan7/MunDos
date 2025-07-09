using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const float DEACTIVATE_TIME = 0.1f;

    [Header("References")]
    [SerializeField] private string dialogueHolderName = "Dialogue Holder";
    [HideInInspector] private DialogueManager dialogueHolder;
    [SerializeField] private Dialogue dialogue;

    [Header("Properties")]
    [SerializeField] private bool startOnEnable;
    [SerializeField] private bool startFromTrigger;
    [SerializeField] private bool isSign;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private GameObject nextDialogue;

    [Header("Timers")]
    [SerializeField] private float nextDialogueTime = 0.5f;
    [HideInInspector] private float deactivateTimer;
    [HideInInspector] private float nextDialogueTimer;

    [Header("Flags")]
    private bool dialogueIsTriggered;

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnEnable()
    {
        ResetValues();

        if (startOnEnable) TriggerDialogue();
    }

    private void Update()
    {
        if (dialogueHolder == null || dialogueHolder.open || !dialogueIsTriggered) return;

        if (nextDialogue != null)
        {
            WaitForOtherDialogue();

            if (nextDialogueTimer <= 0) ActivateOtherDialogue(nextDialogue);
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

        if (col.gameObject.CompareTag("Protag"))
        {
            TriggerDialogue();
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (!startFromTrigger) return;

        if (col.gameObject.CompareTag("Protag"))
        {
            if (isSign) dialogueHolder.EndDialogue();
            if (isSign && deactivateAfter) Deactivate();
        }
    }

    // Helper Functions --------------------------------------------------------

    public void TriggerDialogue()
	{
        if (dialogueHolder == null) print("ERROR: No Dialogue Holder Found");

        dialogueIsTriggered = true;

        StartCoroutine(dialogueHolder.StartDialogue(dialogue));
    }

    void WaitForOtherDialogue()
    {
        dialogueHolder.EndDialogue();
        nextDialogueTimer -= Time.deltaTime;
    }

    public void ActivateOtherDialogue(GameObject nextDialogue)
    {
        nextDialogue.transform.position = GameObject.FindGameObjectWithTag("Protag").transform.position;
        nextDialogue.SetActive(true);

        if (deactivateAfter) gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        if (nextDialogue != null) return;

        deactivateTimer -= Time.deltaTime;
        if (deactivateTimer <= 0) gameObject.SetActive(false);
    }

    public void ResetValues()
    {
        deactivateTimer = DEACTIVATE_TIME;
        nextDialogueTimer = nextDialogueTime;

        dialogueIsTriggered = false;
    }

    void InitializeReferences()
    {
        dialogueHolder = GameObject.Find(dialogueHolderName).GetComponent<DialogueManager>();
    }
}
