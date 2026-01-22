using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    const string SAVE_KEY = "SAVE_DATA";

    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;

    private void Start()
    {
        LoadGame();
        // Automatically load the game when it starts but maybe change soon..
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.objectStates = new List<ObjectSaveData>();

        foreach (ObjectState obj in FindObjectsOfType<ObjectState>(true))
        {
            UniqueID uid = obj.GetComponent<UniqueID>();
            if (uid == null) continue;

            data.objectStates.Add(obj.CaptureState());
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
    }



    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.Log("No save found");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));

        foreach (ObjectSaveData saved in data.objectStates)
        {
            ObjectState obj = FindObjByID(saved.objectID);
            if (obj != null) obj.RestoreState(saved);
        }

        Debug.Log("Game Loaded");
    }


    ObjectState FindObjByID(string id)
    {
        foreach (ObjectState obj in FindObjectsOfType<ObjectState>(true))
        {
            UniqueID uid = obj.GetComponent<UniqueID>();
            if (uid != null && uid.ID == id) return obj;
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveKey)) SaveGame();
        if (Input.GetKeyDown(loadKey)) LoadGame();
    }
}
