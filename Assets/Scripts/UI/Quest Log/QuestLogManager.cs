using UnityEngine;
using TMPro;

public class QuestLogManager : MonoBehaviour
{
    // Variables ---------------------------------------------------------------

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI actualQuestText;
    [SerializeField] private TextMeshProUGUI guideText;

    // Main Functions ----------------------------------------------------------

    // Helper Functions --------------------------------------------------------

    public void ChangeQuest(string newQuest, string newGuide) // To be used by ProgressPoint.cs
    {
        actualQuestText.text = newQuest;
        guideText.text = newGuide;
    }
}
