using TMPro;
using UnityEngine;

/*
 * This is to easily share quest text data between timelines.
 */

[CreateAssetMenu(fileName = "QuestLogData", menuName = "Scriptable Objects/QuestLogData")]
public class QuestLogData : ScriptableObject
{
    public string actualQuestText; 
    public string guideText;
}
