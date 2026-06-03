[System.Serializable]

public class QuestLogSaveData : ComponentSaveData
{
    public string actualQuestTextData;
    public string guideTextData;

    public QuestLogSaveData()
    {
        type = "QuestLog";
    }
}