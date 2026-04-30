[System.Serializable]

public class GameManagerSaveData : ComponentSaveData
{
    public bool gameManagerIsFocusing;
    public bool gameManagerIsHidingUI;
    public bool gameManagerIsPaused;
    public bool gameManagerCanPause;

    public GameManagerSaveData()
    {
        type = "GameManager";
    }
}
