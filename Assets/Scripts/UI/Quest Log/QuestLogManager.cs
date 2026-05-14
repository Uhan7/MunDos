using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class QuestLogManager : MonoBehaviour
{
    // Variables ---------------------------------------------------------------

    [Header("Text Fields")]
    [SerializeField] public TextMeshProUGUI actualQuestText; // For ObjectState.cs
    [SerializeField] public TextMeshProUGUI guideText; // For ObjectState.cs

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

    // Helper Functions --------------------------------------------------------

    public void ChangeQuest(string newQuest, string newGuide) // To be used by ProgressPoint.cs
    {
        actualQuestText.text = newQuest;
        guideText.text = newGuide;

        FlashQuestButton();
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
