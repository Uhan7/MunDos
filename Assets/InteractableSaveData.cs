[System.Serializable]

public class InteractableSaveData : ComponentSaveData
{
    public bool alreadyCheckedConditional;
    public bool alreadyCheckedUnlock;
    public bool alreadyCheckedLock;

    public InteractableSaveData()
    {
        type = "Interactable";
    }
}