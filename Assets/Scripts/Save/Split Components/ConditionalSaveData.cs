[System.Serializable]

public class ConditionalSaveData : ComponentSaveData
{
    public int conditionalObjectChecks;

    public ConditionalSaveData()
    {
        type = "Conditional";
    }
}