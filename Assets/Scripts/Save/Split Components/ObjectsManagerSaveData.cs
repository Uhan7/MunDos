[System.Serializable]

public class ObjectsManagerSaveData : ComponentSaveData
{
    public bool objectsManagerAlreadyEnabled;
    public bool objectsManagerAlreadyCheckedUnlock;
    public bool objectsManagerAlreadyCheckedLock;

    public ObjectsManagerSaveData()
    {
        type = "ObjectsManager";
    }
}