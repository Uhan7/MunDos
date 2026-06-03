using UnityEngine;

public class ProgressionPoint : MonoBehaviour
{
    /*
    INFO: 
    Progression Points are pretty much just to mark when the quest will change
    They will most likely be put on DZs since DZs are the main point where progress
    has been changed.
    */



    // Variables ---------------------------------------------------------------

    [Header("Constants")]
    [HideInInspector] private const string QUEST_LOG_HOLDER_NAME = "Quest Log";

    [Header("References")]
    [HideInInspector] private QuestLogManager questLogManager;

    [Header("Texts to Pass")]
    [SerializeField, TextArea(1, 2)] private string questText;
    [SerializeField, TextArea(2, 3)] private string guideText;

    [Header("When to Pass")]
    [SerializeField] private bool passQuestOnActivate = true;
    [SerializeField] private bool passQuestOnDeactivate;

    private bool hasBeenActivated = false;



    // Main Functions ----------------------------------------------------------

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnEnable()
    {
        if (passQuestOnActivate && !hasBeenActivated)
        {
            PassQuestDetails();
            hasBeenActivated = true;
        }
    }

    private void OnDisable()
    {
        if (passQuestOnDeactivate && !hasBeenActivated)
        {
            PassQuestDetails();
            hasBeenActivated = true;
        }
    }



    // Helper Functions --------------------------------------------------------

    private void InitializeReferences()
    {
        questLogManager = GameObject.Find(QUEST_LOG_HOLDER_NAME).GetComponent<QuestLogManager>();
    }

    private void PassQuestDetails()
    {
        questLogManager.ChangeQuest(questText, guideText);
    }
}
