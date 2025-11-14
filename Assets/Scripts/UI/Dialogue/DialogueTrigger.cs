using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const float DEACTIVATE_TIME = 0.1f;
    [HideInInspector] private const string PROTAG_TAG = "Protag";
    [HideInInspector] private const string DIALOGUE_HOLDER_NAME = "Dialogue Holder";
    [HideInInspector] private const string SFX_SOURCE_NAME = "SFX Source";

    [Header("References")]
    [HideInInspector] private DialogueManager dialogueHolder;
    [SerializeField] public Dialogue dialogue; // Used in Oil.cs

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
    [SerializeField] private bool startFromTrigger;
    [SerializeField] private bool isSign;
    [SerializeField] private bool willFocus;
    [SerializeField] private bool willHideUI;
    [SerializeField] private bool deactivateAfter = true;
    [SerializeField] private bool linksToOtherDialogue;
    [ShowIf("linksToOtherDialogue")] [SerializeField] private GameObject nextDialogue;
    [ShowIf("linksToOtherDialogue")] [SerializeField] private float nextDialogueTime = 0.5f;
    [SerializeField] private AnimOptions playClosingAnimation = AnimOptions.defaultClose;
    [SerializeField] private bool mainCharacterIsSpeaking = true;

    [Header("GameObject Modifications")]
    [SerializeField] private GameObject[] objectsToActivateAfter;
    [SerializeField] private GameObject[] objectsToDeactivateAfter;
    [SerializeField] private GameObject[] conditionalObjectsToCheckAfter;
    [SerializeField] private AudioClip[] soundsToPlayAfter;

    [Header("Timers")]
    [HideInInspector] private float deactivateTimer;
    [HideInInspector] private float nextDialogueTimer;

    [Header("Flags")]
    [HideInInspector] private bool dialogueIsTriggered;
    [HideInInspector] private bool alreadyActivatedObjects = false;
    [HideInInspector] private bool alreadyDeactivatedObjects = false;
    [HideInInspector] private bool alreadyCheckedConditional = false;
    [HideInInspector] private bool alreadyPlayedSounds = false;

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

        if (objectsToActivateAfter.Length != 0 && !alreadyActivatedObjects) ActivateOtherObjects();
        if (objectsToDeactivateAfter.Length != 0 && !alreadyDeactivatedObjects) DeactivateOtherObjects();
        if (conditionalObjectsToCheckAfter.Length != 0 && !alreadyCheckedConditional) AddConditionalCheck();
        if (soundsToPlayAfter.Length != 0 && !alreadyPlayedSounds) PlaySounds();

        if (linksToOtherDialogue && nextDialogue != null)
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

        else
        {
            if (willFocus) Focus(false);
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

        if (dialogueHolder == null || !dialogueHolder.gameObject.activeInHierarchy) dialogueHolder = GameObject.Find(DIALOGUE_HOLDER_NAME).GetComponent<DialogueManager>();
        if (dialogueHolder == null || !dialogueHolder.gameObject.activeInHierarchy) Debug.LogError("Dialogue Holder not Found.");

        dialogueHolder.playClosingAnimation = closeAnim;
        dialogueHolder.mainCharacterIsSpeaking = mainCharacterIsSpeaking;

        if (willFocus) Focus(true);
        if (willHideUI) HideUI(true);

        dialogueIsTriggered = true;
        if (startFromTrigger && !isSign) GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(dialogueHolder.StartDialogue(dialogue));
    }

    void WaitForOtherDialogue()
    {
        dialogueHolder.EndDialogue();
        nextDialogueTimer -= Time.deltaTime;
    }

    public void ActivateOtherDialogue(GameObject nextDialogue)
    {
        nextDialogue.transform.position = GameObject.FindGameObjectWithTag(PROTAG_TAG).transform.position;
        nextDialogue.SetActive(true);

        if (deactivateAfter) gameObject.SetActive(false);
    }

    public void AddConditionalCheck()
    {
        if (alreadyCheckedConditional) return;

        foreach (GameObject conditionalObject in conditionalObjectsToCheckAfter)
        {
            ConditionalObject conditionalObjectScript = conditionalObject.GetComponent<ConditionalObject>();

            conditionalObjectScript.currentChecks++;
            if (conditionalObjectScript.currentChecks >= conditionalObjectScript.requiredChecks) conditionalObject.SetActive(true);
        }

        alreadyCheckedConditional = true;
    }

    public void PlaySounds()
    {
        if (alreadyPlayedSounds) return;

        AudioSource audioSource = GameObject.Find(SFX_SOURCE_NAME).GetComponent<AudioSource>();

        foreach (AudioClip sound in soundsToPlayAfter) audioSource.PlayOneShot(sound);

        alreadyPlayedSounds = true;
    }

    void ActivateOtherObjects()
    {
        foreach (GameObject objs in objectsToActivateAfter) objs.SetActive(true);
        alreadyActivatedObjects = true;
    }

    void DeactivateOtherObjects()
    {
        foreach (GameObject objs in objectsToDeactivateAfter) objs.SetActive(false);
        alreadyDeactivatedObjects = true;
    }

    public void Deactivate()
    {
        if (willFocus)
        {
            Focus(false);

            ZoomEnviManager[] zoomCanvasScripts = FindObjectsByType<ZoomEnviManager>(FindObjectsSortMode.None);
            foreach (ZoomEnviManager zoomCanvasScript in zoomCanvasScripts)
            {
                if (zoomCanvasScript.activated) zoomCanvasScript.DeactivateZoomedEnvi();
            }
        }

        if (willHideUI) HideUI(false);

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
        dialogueHolder = GameObject.Find(DIALOGUE_HOLDER_NAME).GetComponent<DialogueManager>();
    }

    void Focus(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_FOCUSING_DIALOGUE, value);

        EventBroadcaster.Instance.PostEvent(EventNames.FOCUS_DIALOGUE, param);
    }

    void HideUI(bool value)
    {
        Parameters param = new Parameters();
        param.PutExtra(ParamNames.IS_HIDING_UI, value);

        EventBroadcaster.Instance.PostEvent(EventNames.HIDE_UI, param);
    }
}
