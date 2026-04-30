[System.Serializable]

public class AutoMoveZoneSaveData : ComponentSaveData
{
    public AutoMove.Direction autoMoveZoneDirection;

    public AutoMoveZoneSaveData()
    {
        type = "AutoMoveZone";
    }
}
