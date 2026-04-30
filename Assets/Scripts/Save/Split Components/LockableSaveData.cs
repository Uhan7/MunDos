[System.Serializable]

public class LockableSaveData : ComponentSaveData
{
    public bool isLockedAtStart;
    public bool isAlreadyLocked;
    public int lockableObjectChecks;
    public bool hasStoredLockValue;
    public bool storedLockValue;

    public LockableSaveData()
    {
        type = "Lockable";
    }
}
