[System.Serializable]

public class MultiInteractableSaveData : ComponentSaveData
{

    public bool hasActivatedOnce;
    public int interactionsCounter;

    public MultiInteractableSaveData()
    {
        type = "MultiInteractable";
    }
}