using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    [Header("Past Settings")]
    [SerializeField] UnityEngine.UI.Slider pastMusicVol;
    [SerializeField] UnityEngine.UI.Slider pastSFXVol;
    [SerializeField] UnityEngine.UI.Slider pastDialogueVol;

    [Header("Present Settings")]
    [SerializeField] UnityEngine.UI.Slider presentMusicVol;
    [SerializeField] UnityEngine.UI.Slider presentSFXVol;
    [SerializeField] UnityEngine.UI.Slider presentDialogueVol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMusicVol(float vol)
    {
        pastMusicVol.value = vol;
        presentMusicVol.value = vol;
    }
    public void setSFXVol(float vol)
    {
        pastSFXVol.value = vol;
        presentSFXVol.value = vol;
    }
    public void setDialogueVol(float vol)
    {
        pastDialogueVol.value = vol;
        presentDialogueVol.value = vol;
    }
}
