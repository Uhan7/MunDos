using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue To Display with and Use
    private GameObject dialogueHolder;
    [SerializeField] private Dialogue dialogue;

    // Properties of this DialogueTrigger
    [SerializeField] private bool startOnEnable;
    [SerializeField] private bool startFromTrigger;
    [SerializeField] private bool startFromInteract;
    [SerializeField] private bool isSign;
    [SerializeField] private GameObject nextDialogue;

    // Timers
    private const float DEACTIVATE_TIME = 0.1f;
    private float deactivateTimer;
    [SerializeField] private float nextDialogueTime = 0.5f;
    private float nextDialogueTimer;

    // Destroying / Deactivating of this DialogueTrigger
    [SerializeField] private bool deactivateAfter;

    // Flags
    private bool dialogueIsTriggered;

    private void Awake()
    {
        //if (dialogueHolder == null) dialogueHolder = GameObject.Find("Common Dialogue Holder");
    }

    private void OnEnable()
    {
        ResetFlags();

        if (startOnEnable) TriggerDialogue();
    }

    private void Update()
    {
        if (dialogueHolder != null && !dialogueHolder.GetComponent<DialogueManager>().open)
        {
            //if (nextDialogue != null)
            //{
            //    if (finishedDialogue) nextDialogueTimer -= Time.deltaTime;
            //    if (nextDialogueTimer <= 0 && !triggeredOtherDialogue) ActivateOtherDialogue(nextDialogue);
            //}

            if (!dialogueIsTriggered) return;

            if (nextDialogue != null)
            {
                nextDialogue.SetActive(false);
                nextDialogueTimer -= Time.deltaTime;

                // Constantly deactivate dialogue until you activate next one to ensure no overlaps
                dialogueHolder.GetComponent<DialogueManager>().EndDialogue();

                if (nextDialogueTimer <= 0)
                {
                    ActivateOtherDialogue(nextDialogue);
                }
            }

            else if (deactivateAfter)
            {
                dialogueHolder.GetComponent<DialogueManager>().EndDialogue();
                Deactivate();
            }
        }
    }

    public void TriggerDialogue()
	{
        if (dialogueHolder == null) dialogueHolder = GameObject.Find("Dialogue Holder");
        if (dialogueHolder == null) print("ERROR: No Dialogue Holder Found");

        StartCoroutine(dialogueHolder.GetComponent<DialogueManager>().StartDialogue(dialogue));

        dialogueIsTriggered = true;
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

        if (col.gameObject.CompareTag("Protag")) {
            if (isSign) dialogueHolder.GetComponent<DialogueManager>().EndDialogue();
            if (isSign && deactivateAfter) Deactivate();
        }
    }

    public void ActivateOtherDialogue(GameObject nextDialogue)
    {
        nextDialogue.transform.position = GameObject.FindGameObjectWithTag("Protag").transform.position;
        nextDialogue.SetActive(true);

        if (deactivateAfter) gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        // Only allow self deactivation when there are no Next Dialogues
        if (nextDialogue != null) return;

        deactivateTimer -= Time.deltaTime;
        if (deactivateTimer <= 0) gameObject.SetActive(false);
    }

    public void ResetFlags()
    {
        deactivateTimer = DEACTIVATE_TIME;
        nextDialogueTimer = nextDialogueTime;

        dialogueIsTriggered = false;
    }
}
