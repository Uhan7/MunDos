[System.Serializable]

public class ProgressionPointSaveData : ComponentSaveData
{
    public bool hasBeenActivated;

    public ProgressionPointSaveData()
    {
        type = "ProgressionPoint";
    }
}