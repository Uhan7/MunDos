using NaughtyAttributes;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QuestLogManager : MonoBehaviour
{
    // Variables ---------------------------------------------------------------

    [Header("Text Fields")]
    [SerializeField] public TextMeshProUGUI actualQuestText; // For ObjectState.cs
    [SerializeField] public TextMeshProUGUI guideText; // For ObjectState.cs
    public QuestLogData data;

    [Header("Anim Reference")]
    [SerializeField] private GameObject questLogButton;
    [SerializeField] private DialogueManager dialogueManager; // To make quest log close once a dialogue starts

    // Animator
    private Animator animator;
    private Animator buttonAnimator;
    private bool isOpen = false;

    // Main Functions ----------------------------------------------------------
    private void Start()
    {
        animator = GetComponent<Animator>();
        buttonAnimator = questLogButton.GetComponent<Animator>();
    }

    private void Update()
    {
        if (dialogueManager.open && isOpen)
        {
            ToggleQuestLog();
        }
    }

    private void OnEnable()
    {
        UpdateQuests();
    }

    // Helper Functions --------------------------------------------------------

    public void ChangeQuest(string newQuest, string newGuide) // To be used by ProgressPoint.cs
    {
        data.actualQuestText = newQuest;
        data.guideText = newGuide;

        UpdateQuests();

        FlashQuestButton();
    }

    public void UpdateQuests()
    {
        if (actualQuestText.text != data.actualQuestText)
        {
            actualQuestText.text = data.actualQuestText;
            guideText.text = data.guideText;
        }
    }

        // UI Anims
    public void ToggleQuestLog()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    public void FlashQuestButton()
    {
        if (buttonAnimator != null) buttonAnimator.ResetTrigger("Flash");
        if (buttonAnimator != null) buttonAnimator.SetTrigger("Flash");
    }

    public void OnDisable()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
    }
}
