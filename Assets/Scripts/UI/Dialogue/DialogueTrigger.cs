using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue To Display with and Use
    private GameObject dialogueHolder;
	public Dialogue dialogue;

    // Properties of this DialogueTrigger
    public bool startFromTrigger;
    [SerializeField] private bool startFromInteract;
    public bool sign;
    public bool repeatable;
    public GameObject nextDialogue;
    public float timeTillDeactivate = 0.1f;
    public float timeTillNextDialogue;

    // Destroying / Deactivating of this DialogueTrigger
    public bool destroyImmediate;
    public bool destroyAfter;
    public bool deactivateAfter;

    // Current state of this DialogueTrigger
    private bool alreadyTriggered;

    private void Awake()
    {
        //if (dialogueHolder == null) dialogueHolder = GameObject.Find("Common Dialogue Holder");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (dialogueHolder != null && dialogueHolder.GetComponent<DialogueManager>().open == false && alreadyTriggered)
        {
            if (nextDialogue != null && timeTillNextDialogue > 0)
            {
                StartOtherDialogue(nextDialogue);
                // return;
            }

            if (destroyAfter)
            {
                dialogueHolder.GetComponent<DialogueManager>().EndDialogue();

                // Because theres a diff destroy call if u call another shit
                if (nextDialogue == null) Destroy(gameObject, .1f);
            }

            if (deactivateAfter)
            {
                Deactivate();
            }
        }
    }

    public void TriggerDialogue()
	{
        if (alreadyTriggered && !repeatable) return;

        //if (dialogueHolder == null) dialogueHolder = GameObject.Find("Past Dialogue Holder");
        //if (dialogueHolder == null) dialogueHolder = GameObject.Find("Present Dialogue Holder");
        if (dialogueHolder == null) dialogueHolder = GameObject.Find("Dialogue Holder");
        if (dialogueHolder == null) print("help");

        StartCoroutine(dialogueHolder.GetComponent<DialogueManager>().StartDialogue(dialogue));
        if (destroyImmediate && !sign) Destroy(gameObject, .1f);

        alreadyTriggered = true;
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
            if (sign && !destroyAfter) dialogueHolder.GetComponent<DialogueManager>().EndDialogue();
            if (sign && deactivateAfter) Deactivate();
            //GameObject.FindGameObjectWithTag("Protag").GetComponent<PlayerMove>().canMove = true;
        }
    }

    public void StartOtherDialogue(GameObject nextDialogue)
    {
        nextDialogue.SetActive(false);
        timeTillNextDialogue -= Time.deltaTime;
        if (timeTillNextDialogue < 0)
        {
            nextDialogue.transform.position = GameObject.FindGameObjectWithTag("Protag").transform.position;
            nextDialogue.SetActive(true);

            if (destroyAfter) Destroy(gameObject, .1f);
        }
    }

    public void Deactivate()
    {
        // if (stopPlayer) GameObject.Find("PlayerMove").GetComponent<PlayerMove>().canMove = true;
        dialogueHolder.GetComponent<DialogueManager>().EndDialogue();

        gameObject.SetActive(false);

        if (repeatable) alreadyTriggered = false;
    }

    //public void OnCollisionExit2D(Collision2D col)
    //{
    //    if (startFromTrigger) return;

    //    if (col.gameObject.CompareTag("Protag"))
    //    {
    //        if (sign && !destroyAfter) dialogueHolder.GetComponent<DialogueManager>().EndDialogue();
    //        else if (sign && destroyAfter)
    //        {
    //            dialogueHolder.GetComponent<DialogueManager>().EndDialogue();
    //            Destroy(gameObject, .1f);
    //        }
    //    }
    //}

}
