using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string SAVE_KEY = "SAVE_DATA";
    [HideInInspector] private const string SFX_SOURCE_NAME = "SFX Source";

    [Header("Keycodes (Debug)")]
    [Tooltip("Keybinds may not work properly."), SerializeField] private bool allowKeybinds;
    [SerializeField] private KeyCode saveKey = KeyCode.Alpha6;
    [SerializeField] private KeyCode loadKey = KeyCode.Alpha7;

    [Header("References")]
    [HideInInspector] private AudioSource sfxSource;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private PlayerInteract pastPlayerInteract;
    [SerializeField] private PlayerInteract presentPlayerInteract;
    [SerializeField] private PlayerMove pastPlayerMove;
    [SerializeField] private PlayerMove presentPlayerMove;

    [Header("Audio Stuff")]
    [SerializeField] private AudioClip saveSFX;
    [SerializeField] private AudioClip loadSFX;

    private void Awake()
    {
        // LoadGame();
        // Automatically load the game when it starts but maybe change soon..

        sfxSource = GameObject.Find(SFX_SOURCE_NAME).GetComponent<AudioSource>();

        if (SettingsInfo.fromContinue)
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        SaveData data = new();

        // Physical Objects
        data.objectStates = new List<ObjectSaveData>();
        foreach (ObjectState obj in FindObjectsOfType<ObjectState>(true)) data.objectStates.Add(obj.CaptureState());

        // Logical/Data
        data.players = new List<PlayerSaveData>();
        data.players.Add(CapturePlayer(pastPlayerInteract, pastPlayerMove, Timeline.Past));
        data.players.Add(CapturePlayer(presentPlayerInteract, presentPlayerMove, Timeline.Present));

        // Progression Flags
        data.timelineUnlocked = timelineManager.timelineUnlocked;
        data.currentTimeline = timelineManager.currentTimeline;

        // Actual Save
        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
        Debug.Log("Save size (chars): " + JsonUtility.ToJson(data).Length);
        Debug.Log("Save size (bytes approx): " + System.Text.Encoding.UTF8.GetByteCount(JsonUtility.ToJson(data)));

        sfxSource.PlayOneShot(saveSFX); // Plays last to let players know it saved
    }


    public void LoadGame()
    {
        sfxSource.PlayOneShot(loadSFX); // Plays first to let players know it loading

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
        foreach (PlayerSaveData playerSaveData in data.players)
        {
            if (playerSaveData.protagTimeline == Timeline.Past) RestorePlayer(pastPlayerInteract, pastPlayerMove, playerSaveData);
            else if (playerSaveData.protagTimeline == Timeline.Present) RestorePlayer(presentPlayerInteract, presentPlayerMove, playerSaveData);
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

    PlayerSaveData CapturePlayer(PlayerInteract playerInteract, PlayerMove playerMove, Timeline protagTimeline)
    {
        PlayerSaveData data = new PlayerSaveData();
        data.itemDatas = new List<ItemData>();

        // Timeline
        data.protagTimeline = protagTimeline;

        // Items
        data.playerItemIndex = playerInteract.playerItemIndex;
        foreach (ItemData item in playerInteract.itemDatas) data.itemDatas.Add(new ItemData(item));

        // Player Move
        data.moveCanInput = playerMove.canInput;
        data.moveCanMove = playerMove.canMove;

        // Player Interact
        data.interactWillUpdate = playerInteract.willUpdate;
        data.interactIsEmpty = playerInteract.isEmpty;
        data.interactHasCheckedInventory = playerInteract.hasCheckedInventory;
        data.interactCanInput = playerInteract.canInput;
        data.interactHasActiveItem = playerInteract.hasActiveItem;
        data.interactIsSelected = playerInteract.isSelected;
        data.interactIsZoomed = playerInteract.isZoomed;
        data.interactActiveObjectIndex = playerInteract.activeObjectIndex;

        return data;
    }

    void RestorePlayer(PlayerInteract playerInteract, PlayerMove playerMove, PlayerSaveData data)
    {
        // Items
        playerInteract.playerItemIndex = data.playerItemIndex;
        for (int i = 0; i < data.itemDatas.Count; i++) playerInteract.itemDatas[i] = new ItemData(data.itemDatas[i]);

        // Player Move
        playerMove.canInput = data.moveCanInput;
        playerMove.canMove = data.moveCanMove;

        // Player Interact
        playerInteract.willUpdate = data.interactWillUpdate;
        playerInteract.isEmpty = data.interactIsEmpty;
        playerInteract.hasCheckedInventory = data.interactHasCheckedInventory;
        playerInteract.canInput = data.interactCanInput;
        playerInteract.hasActiveItem = data.interactHasActiveItem;
        playerInteract.isSelected = data.interactIsSelected;
        playerInteract.isZoomed = data.interactIsZoomed;
        playerInteract.activeObjectIndex = data.interactActiveObjectIndex;
    }

    private void Update()
    {
        if (!allowKeybinds) return;

        if (Input.GetKeyDown(saveKey)) SaveGame();
        if (Input.GetKeyDown(loadKey)) LoadGame();
    }
}
