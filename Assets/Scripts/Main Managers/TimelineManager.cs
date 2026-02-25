using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TimelineManager : MonoBehaviour
{
    [Header("Constants")]
    [HideInInspector] private const string DIALOGUE_HOLDER_NAME = "Dialogue Holder";
    [HideInInspector] private const string CURRENT_PROTAG_TAG = "Protag";

    [Header("References")]
    [HideInInspector] private DialogueManager dialogueHolder;

    [Header("Key Inputs")]
    [SerializeField] private KeyCode switchTimelineKey;
    [SerializeField] private KeyCode interactKey;

    [Header("Settings")]
    [SerializeField] private float cooldownTime = 1f;
    [HideInInspector] private float cooldownTimer;

    [Header("Timelines")]
    [SerializeField] private GameObject pastTimeline;
    [SerializeField] private GameObject presentTimeline;

    [Header("Visuals")]
    [SerializeField] private GameObject presentTransition;
    [SerializeField] private GameObject pastTransition;

    [Header("Audio")]
    [SerializeField] private Room[] pastRooms;
    [SerializeField] private Room[] presentRooms;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource dialogueSFXSource;
    [SerializeField] private AudioClip transitionSoundToPast;
    [SerializeField] private AudioClip transitionSoundToPresent;

    [Header("Flags")]
    [SerializeField] public int currentTimeline = 1; // 0 is Past | 1 is Present -- Used for Save
    [HideInInspector] public bool canSwitch = true; // Used in GameManager.cs
    [SerializeField] public bool timelineUnlocked = false;

    void Start()
    {
        // if (pastTimeline.activeInHierarchy) currentTimeline = 0;
        // else currentTimeline = 1;
    }

    void Update()
    {
        TimerUpdate();
        SettingsUpdate();

        if (!canSwitch || !timelineUnlocked) return;
        if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.L)) return;

        if (Input.GetKeyDown(switchTimelineKey) && !Input.GetKey(interactKey) && cooldownTimer <= 0)
        {
            cooldownTimer = cooldownTime;

            PlayerMove playerMoveScript = GameObject.FindGameObjectWithTag(CURRENT_PROTAG_TAG).GetComponent<PlayerMove>();
            if (!playerMoveScript.canInput) return;

            StartCoroutine(SwitchTimeline(0.00f));
        }
    }

    // Update Functions --------------------------------------------------------

    void SettingsUpdate()
    {
        foreach (Room room in pastRooms)
        {
            if (room.roomCamera.activeInHierarchy && room.connectedAudioSource != null) room.connectedAudioSource.volume = (currentTimeline == 0) ? SettingsInfo.musicVol : 0;
        }
        foreach (Room room in presentRooms)
        {
            if (room.roomCamera.gameObject.activeInHierarchy && room.connectedAudioSource != null) room.connectedAudioSource.volume = (currentTimeline == 1) ? SettingsInfo.musicVol : 0;
        }
        sfxSource.volume = SettingsInfo.SFXVol;
        dialogueSFXSource.volume = SettingsInfo.dialogueVol;
    }

    void TimerUpdate()
    {
        cooldownTimer -= Time.deltaTime;
    }

    IEnumerator SwitchTimeline(float delayTime)
    {
        // Delay
        yield return new WaitForSeconds(delayTime);

        // modify this area to have cooler effects and stuff

        if (currentTimeline == 0)
        {
            pastTimeline.SetActive(false);
            presentTimeline.SetActive(true);

            currentTimeline = 1;
        }
        else
        {
            pastTimeline.SetActive(true);
            presentTimeline.SetActive(false);

            currentTimeline = 0;
        }

        TimelineTransition();
    }

    void TimelineTransition()
    {
        if (currentTimeline == 0)
        {
            pastTransition.SetActive(true);
            presentTransition.SetActive(false);
            sfxSource.PlayOneShot(transitionSoundToPast);
        }
        else
        {
            pastTransition.SetActive(false);
            presentTransition.SetActive(true);
            sfxSource.PlayOneShot(transitionSoundToPresent);
        }
    }

    IEnumerator FadeBGM(AudioSource fadeOut, AudioSource fadeIn, float fadeDuration)
    {
        float time = 0f;
        float startVolOut = fadeOut.volume;
        float startVolIn = fadeIn.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            fadeOut.volume = Mathf.Lerp(startVolOut, 0f, t);
            fadeIn.volume = Mathf.Lerp(startVolIn, SettingsInfo.musicVol, t);

            yield return null;
        }

        fadeOut.volume = 0f;
        fadeIn.volume = SettingsInfo.musicVol;
    }

    // Helper (Get) Function --------------------------------------------------------
    public int GetCurrentTimeline() // Used in SlotHander
    {
        return currentTimeline;
    }
}
