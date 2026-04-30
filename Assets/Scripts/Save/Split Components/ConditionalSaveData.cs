[System.Serializable]

public class ConditionalSaveData : ComponentSaveData
{
    public bool conditionalState;

    public ConditionalSaveData()
    {
        type = "Conditional";
    }
}