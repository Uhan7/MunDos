[System.Serializable]

public class ObjectsManagerSaveData : ComponentSaveData
{
    public bool objectManagerAlreadyEnabled;
    public bool objectManagerAlreadyCheckedUnlock;
    public bool objectManagerAlreadyCheckedLock;

    public ObjectsManagerSaveData()
    {
        type = "ObjectsManager";
    }
}
