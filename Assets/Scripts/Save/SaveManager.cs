using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string SAVE_KEY = "SAVE_DATA";
    [HideInInspector] private const string SFX_SOURCE_NAME = "SFX Source";

    [Header("Keycodes (Debug)")]
    [Tooltip("Keybinds may not work properly."), SerializeField] private bool allowKeybinds;
    [SerializeField] private KeyCode saveKey = KeyCode.Alpha6;
    [SerializeField] private KeyCode loadKey = KeyCode.Alpha7;
    [SerializeField] private KeyCode writeKey = KeyCode.Alpha8;

    [Header("References")]
    [HideInInspector] private AudioSource sfxSource;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private SceneTransitioner sceneTransitioner;
    [SerializeField] private PauseMenuManager pauseMenuManagerPast;
    [SerializeField] private PauseMenuManager pauseMenuManagerPresent;
    [SerializeField] private PlayerInteract pastPlayerInteract;
    [SerializeField] private PlayerInteract presentPlayerInteract;
    [SerializeField] private PlayerMove pastPlayerMove;
    [SerializeField] private PlayerMove presentPlayerMove;

    [Header("Audio Stuff")]
    [SerializeField] private AudioClip saveSFX;
    [SerializeField] private AudioClip loadSFX;

    [Header("Save Stuff")]
    [SerializeField, ReadOnly] public static bool load_on_start = false;
    [SerializeField, ReadOnly] public static bool read_load_on_start = false;
    [SerializeField, ReadOnly] public static TextAsset checkpointFile;

    private void Awake()
    {
        sfxSource = GameObject.Find(SFX_SOURCE_NAME).GetComponent<AudioSource>();
        if (load_on_start) StartCoroutine(LoadGame());
        if (read_load_on_start) ReadAndLoadSaveData(checkpointFile);

        //if (SettingsInfo.fromContinue) LoadGame(); -> will temporarily replace this with the static shi on top
    }

    // Main Save/Load Functions ------------------------------------------------
    public void SaveGame()
    {
        SaveData data = new();

        // Physical Objects
        data.objectStates = new List<ObjectSaveData>();
        foreach (ObjectState obj in FindObjectsByType<ObjectState>(FindObjectsInactive.Include, FindObjectsSortMode.None)) data.objectStates.Add(obj.CaptureState());

        // Logical/Data
        data.players = new List<PlayerSaveData>();
        data.players.Add(CapturePlayer(pastPlayerInteract, pastPlayerMove, Timeline.Past));
        data.players.Add(CapturePlayer(presentPlayerInteract, presentPlayerMove, Timeline.Present));

        // Progression Flags
        data.timelineUnlocked = timelineManager.timelineUnlocked;
        data.currentTimeline = timelineManager.currentTimeline;

        sfxSource.PlayOneShot(saveSFX); // Plays last to let players know it saved

        // Actual Save
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
        Debug.Log("Save size (chars): " + json.Length);
        Debug.Log("Save size (bytes): " + System.Text.Encoding.UTF8.GetByteCount(json));

    }

    public void StartLoad()
    {        
        if (sceneTransitioner != null) sceneTransitioner.TriggerLoadingScreen();

        StartCoroutine(LoadGame());

    }

    // If smth breaks in the loading, change this back to public void...
    public IEnumerator LoadGame()
    {
        if (sfxSource != null) sfxSource.PlayOneShot(loadSFX); // Plays first to let players know it loading

        // If none, load normal
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            Debug.Log("No Save Found! Loading as normal.");
            yield return null;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));

        // Set up Camera Stuff
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        var originalBlend = brain.DefaultBlend;
        brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);

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

        // Then Camera Cut
        StartCoroutine(RestoreBlend(brain, originalBlend));

        // Load Debug
        string json = PlayerPrefs.GetString(SAVE_KEY);
        Debug.Log("Game Loaded");

        sceneTransitioner.EndLoadingScreen();

        Debug.Log("Loaded save size (chars): " + json.Length);
        Debug.Log("Loaded save size (bytes): " + System.Text.Encoding.UTF8.GetByteCount(json));

        yield return null;
    }

    // Extra Save/Load Functions -----------------------------------------------
    public void WriteSaveData()
    {
        SaveData data = new();

        // Physical Objects
        data.objectStates = new List<ObjectSaveData>();
        foreach (ObjectState obj in FindObjectsByType<ObjectState>(FindObjectsInactive.Include, FindObjectsSortMode.None)) data.objectStates.Add(obj.CaptureState());

        // Logical/Data
        data.players = new List<PlayerSaveData>();
        data.players.Add(CapturePlayer(pastPlayerInteract, pastPlayerMove, Timeline.Past));
        data.players.Add(CapturePlayer(presentPlayerInteract, presentPlayerMove, Timeline.Present));

        // Progression Flags
        data.timelineUnlocked = timelineManager.timelineUnlocked;
        data.currentTimeline = timelineManager.currentTimeline;

        string folderPath = Application.dataPath + "/Scripts/Save/Checkpoints";
        if (!Directory.Exists(folderPath)) Debug.LogError("Folder named " + folderPath + " doesn't exist.");

        string filePath = folderPath + "/[CHECKPOINT_NAME].json";
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, json);

        Debug.Log("Checkpoint saved!");

        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    public void ReadAndLoadSaveData(TextAsset jsonFile)
    {
        // Debug things for safety check
        if (jsonFile == null)
        {
            Debug.LogError("No checkpoint file!");
            return;
        }
        Debug.Log("Loading Checkpoint : " + jsonFile.name);

        string json = jsonFile.text;
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Set up Camera Stuff
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        var originalBlend = brain.DefaultBlend;
        brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);

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

        // Then Camera Cut
        StartCoroutine(RestoreBlend(brain, originalBlend));

        // Debug
        Debug.Log("Checkpoint successful! Skipped to " + jsonFile.name);
    }

    public void SetCheckpointFile(TextAsset file)
    {
        checkpointFile = file;
    }

    // Other Helper Functions --------------------------------------------------
    ObjectState FindObjByID(string id)
    {
        foreach (ObjectState obj in FindObjectsByType<ObjectState>(FindObjectsInactive.Include, FindObjectsSortMode.None))
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
        for (int i = 0; i < data.itemDatas.Count; i++)
        {
            playerInteract.itemDatas[i] = new ItemData(data.itemDatas[i]);
            playerInteract.itemDatas[i].objectsToInteractWith = new GameObject[playerInteract.itemDatas[i].objectIDsToInteractWith.Length];

            // Replace each item's valid obj reference
            for (int j = 0; j < playerInteract.itemDatas[i].objectIDsToInteractWith.Length; j++)
            {
                playerInteract.itemDatas[i].objectsToInteractWith[j] = FindObjByID(playerInteract.itemDatas[i].objectIDsToInteractWith[j]).gameObject;
            }
        }

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

    IEnumerator RestoreBlend(CinemachineBrain brain, CinemachineBlendDefinition original)
    {
        yield return null; // wait 1 frame
        brain.DefaultBlend = original;
    }

    public void SetLoadOnStart(bool val)
    {
        load_on_start = val;
    }

    public void SetReadLoadOnStart(bool val)
    {
        read_load_on_start = val;
    }

    private void Update()
    {
        if (Input.GetKeyDown(writeKey)) WriteSaveData();

        if (!allowKeybinds) return;

        if (Input.GetKeyDown(saveKey)) SaveGame();
        if (Input.GetKeyDown(loadKey)) StartCoroutine(LoadGame());
    }
}
