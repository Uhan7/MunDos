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

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        switch (audioType)
        {
            case AUDIO_TYPE.BGM:
                audioSource.volume = SettingsInfo.musicVol;
                break;
            case AUDIO_TYPE.SFX:
                audioSource.volume = SettingsInfo.SFXVol;
                break;
            case AUDIO_TYPE.DIALOGUE:
                audioSource.volume = SettingsInfo.dialogueVol;
                break;
        }
    }
}
