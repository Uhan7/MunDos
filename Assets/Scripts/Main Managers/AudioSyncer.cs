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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
