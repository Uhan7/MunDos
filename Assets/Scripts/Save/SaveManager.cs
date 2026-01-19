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
        // Automatically load the game when it starts
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // PLAYER
        // data.playerPosition = Player.Instance.transform.position;
        // data.inventoryItemIDs = Player.Instance.GetInventoryItemIDs();

        // OBJECTS
        data.objectStates = new List<ObjectSaveData>();

        foreach (ObjectState env in FindObjectsOfType<ObjectState>(true))
        {
            UniqueID uid = env.GetComponent<UniqueID>();
            if (uid == null) continue;

            data.objectStates.Add(new ObjectSaveData
            {
                objectID = uid.ID,
                isActive = env.gameObject.activeSelf
            });
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

        SaveData data = JsonUtility.FromJson<SaveData>(
            PlayerPrefs.GetString(SAVE_KEY)
        );

        // PLAYER
        // Player.Instance.transform.position = data.playerPosition;
        // Player.Instance.SetInventoryFromIDs(data.inventoryItemIDs);

        // OBJECTS
        foreach (ObjectSaveData saved in data.objectStates)
        {
            ObjectState obj = FindEnvByID(saved.objectID);
            if (obj != null)
                obj.gameObject.SetActive(saved.isActive);
        }

        Debug.Log("Game Loaded");
    }

    ObjectState FindEnvByID(string id)
    {
        foreach (ObjectState env in FindObjectsOfType<ObjectState>(true))
        {
            UniqueID uid = env.GetComponent<UniqueID>();
            if (uid != null && uid.ID == id)
                return env;
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveKey)) SaveGame();
        if (Input.GetKeyDown(loadKey)) LoadGame();
    }
}
