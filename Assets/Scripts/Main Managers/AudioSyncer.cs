using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    enum AUDIO_TYPE
    {
        BGM,
        SFX,
        DIALOGUE
    };

    [SerializeField] private AUDIO_TYPE audioType;
    [HideInInspector] private AudioSource audioSource;
    [SerializeField] private bool alwaysChangeOnSettings = false;
    [SerializeField] private bool onAtStart = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        switch (audioType)
        {
            case AUDIO_TYPE.BGM:
                if (!onAtStart) audioSource.volume = 0;
                else audioSource.volume = SettingsInfo.musicVol;
                break;
            case AUDIO_TYPE.SFX:
                audioSource.volume = SettingsInfo.SFXVol;
                break;
            case AUDIO_TYPE.DIALOGUE:
                audioSource.volume = SettingsInfo.dialogueVol;
                break;
        }
    }

    private void Update()
    {
        if (alwaysChangeOnSettings) audioSource.volume = SettingsInfo.musicVol;
    }

}
