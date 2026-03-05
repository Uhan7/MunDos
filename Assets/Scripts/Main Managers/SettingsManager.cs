using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private bool isTitleScreen;

    [Header("Title Screen Settings")]
    [ShowIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider musicVol;
    [ShowIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider SFXVol;
    [ShowIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider dialogueVol;

    [Header("Past Settings")]
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider pastMusicVol;
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider pastSFXVol;
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider pastDialogueVol;

    [Header("Present Settings")]
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider presentMusicVol;
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider presentSFXVol;
    [HideIf("isTitleScreen")] [SerializeField] UnityEngine.UI.Slider presentDialogueVol;

    void Start()
    {
        SettingsInfo.musicVol = PlayerPrefs.GetFloat("MUSIC_VOL", 0.75f);
        SettingsInfo.SFXVol = PlayerPrefs.GetFloat("SFX_VOL", 0.75f);
        SettingsInfo.dialogueVol = PlayerPrefs.GetFloat("DIALOGUE_VOL", 1f);

        Sync();
    }

    public void setMusicVol(float vol)
    {
        SettingsInfo.musicVol = vol;
        if (!isTitleScreen)
        {
            pastMusicVol.value = vol;
            presentMusicVol.value = vol;
        } else
        {
            musicVol.value = vol;
        }

        SaveSettings();
    }
    public void setSFXVol(float vol)
    {
        SettingsInfo.SFXVol = vol;
        if (!isTitleScreen)
        {
            pastSFXVol.value = vol;
            presentSFXVol.value = vol;
        } else
        {
            SFXVol.value = vol;
        }

        SaveSettings();
    }
    public void setDialogueVol(float vol)
    {
        SettingsInfo.dialogueVol = vol;
        if (!isTitleScreen)
        {
            pastDialogueVol.value = vol;
            presentDialogueVol.value = vol;
        } else
        {
            dialogueVol.value = vol;
        }

        SaveSettings();
    }

    public void Sync()
    {
        if (!isTitleScreen) StartCoroutine(SyncSettings());
        else StartCoroutine(SyncTitleSettings());
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MUSIC_VOL", SettingsInfo.musicVol);
        PlayerPrefs.SetFloat("SFX_VOL", SettingsInfo.SFXVol);
        PlayerPrefs.SetFloat("DIALOGUE_VOL", SettingsInfo.dialogueVol);
    }

    IEnumerator SyncSettings()
    {
        yield return new WaitForEndOfFrame();

        pastMusicVol.value = SettingsInfo.musicVol;
        presentMusicVol.value = SettingsInfo.musicVol;

        pastSFXVol.value = SettingsInfo.SFXVol;
        presentSFXVol.value = SettingsInfo.SFXVol;

        pastDialogueVol.value = SettingsInfo.dialogueVol;
        presentDialogueVol.value = SettingsInfo.dialogueVol;
    }

    IEnumerator SyncTitleSettings()
    {
        yield return new WaitForEndOfFrame();

        musicVol.value = SettingsInfo.musicVol;
        SFXVol.value = SettingsInfo.SFXVol;
        dialogueVol.value = SettingsInfo.dialogueVol;
    }

    public void ToggleDebugMode()
    {
        SettingsInfo.debugMode = !SettingsInfo.debugMode;
        print("Debug Mode set to: " + SettingsInfo.debugMode);
    }
}
