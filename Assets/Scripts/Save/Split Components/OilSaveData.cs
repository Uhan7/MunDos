[System.Serializable]

public class OilSaveData : ComponentSaveData
{
    public float pH;
    public float salinity;
    public bool finished;

    public OilSaveData()
    {
        type = "Oil";
    }
}