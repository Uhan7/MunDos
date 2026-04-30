[System.Serializable]

public class ColliderSaveData : ComponentSaveData
{
    public bool colliderState;

    public ColliderSaveData()
    {
        type = "Collider";
    }
}