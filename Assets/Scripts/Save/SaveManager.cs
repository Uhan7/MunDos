using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    const string SAVE_KEY = "SAVE_DATA";

    [SerializeField] private KeyCode saveKey = KeyCode.Alpha6;
    [SerializeField] private KeyCode loadKey = KeyCode.Alpha7;

    [SerializeField] private PlayerInteract pastPlayer;
    [SerializeField] private PlayerInteract presentPlayer;

    [Header("References")]
    [SerializeField] private TimelineManager timelineManager;

    private void Start()
    {
        // LoadGame();
        // Automatically load the game when it starts but maybe change soon..
        if (SettingsInfo.fromContinue)
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // Physical Objects
        data.objectStates = new List<ObjectSaveData>();
        foreach (ObjectState obj in FindObjectsOfType<ObjectState>(true)) data.objectStates.Add(obj.CaptureState());

        // Logical/Data
        data.playerItems = new List<PlayerItemSaveData>();
        data.playerItems.Add(CapturePlayer(pastPlayer, Timeline.Past));
        data.playerItems.Add(CapturePlayer(presentPlayer, Timeline.Present));

        // Progression Flags
        data.timelineUnlocked = timelineManager.timelineUnlocked;
        data.currentTimeline = timelineManager.currentTimeline;

        // Actual Save
        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
        Debug.Log("Save size (chars): " + JsonUtility.ToJson(data).Length);
        Debug.Log("Save size (bytes approx): " + System.Text.Encoding.UTF8.GetByteCount(JsonUtility.ToJson(data)));
    }


    public void LoadGame()
    {
        // If none, load normal
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.Log("No Save Found! Loading as normal.");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));

        // Physical Objects
        foreach (ObjectSaveData saved in data.objectStates)
        {
            ObjectState obj = FindObjByID(saved.objectID);
            if (obj != null) obj.RestoreState(saved);
        }

        // Logical/Data
        foreach (PlayerItemSaveData playerItem in data.playerItems)
        {
            if (playerItem.protagTimeline == Timeline.Past) RestorePlayer(pastPlayer, playerItem);
            else if (playerItem.protagTimeline == Timeline.Present) RestorePlayer(presentPlayer, playerItem);
        }

        // Progression Flags
        timelineManager.timelineUnlocked = data.timelineUnlocked;
        timelineManager.currentTimeline = data.currentTimeline;

        Debug.Log("Game Loaded");
    }



    ObjectState FindObjByID(string id)
    {
        foreach (ObjectState obj in FindObjectsOfType<ObjectState>(true))
        {
            UniqueID uid = obj.GetComponent<UniqueID>();
            if (uid == null) print("null obj !!");
            if (uid != null && uid.ID == id) return obj;
        }
        return null;
    }

    ItemData FindItemByID(string id)
    {
        foreach (Item item in FindObjectsOfType<Item>(true))
        {
            UniqueID uid = item.GetComponent<UniqueID>();
            if (uid != null && uid.ID == id)
            {
                return item.GetData();
            }
        }
        return null;
    }


    PlayerItemSaveData CapturePlayer(PlayerInteract player, Timeline protagTimeline)
    {
        PlayerItemSaveData data = new PlayerItemSaveData();

        data.protagTimeline = protagTimeline;
        data.playerItemIndex = player.playerItemIndex;
        data.itemDatas = new List<ItemData>();

        foreach (ItemData item in player.itemDatas)
        {
            data.itemDatas.Add(new ItemData(item));
        }


        return data;
    }

    void RestorePlayer(PlayerInteract player, PlayerItemSaveData data)
    {
        player.playerItemIndex = data.playerItemIndex;

        for (int i = 0; i < data.itemDatas.Count; i++)
        {
            player.itemDatas[i] = new ItemData(data.itemDatas[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveKey)) SaveGame();
        if (Input.GetKeyDown(loadKey)) LoadGame();
    }
}
