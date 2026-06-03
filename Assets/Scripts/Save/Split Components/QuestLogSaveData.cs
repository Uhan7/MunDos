[System.Serializable]

public class QuestLogSaveData : ComponentSaveData
{
    public string actualQuestTextData;
    public string guideTextData;
    public bool hasBeenActivated;

    public QuestLogSaveData()
    {
        type = "QuestLog";
    }
}