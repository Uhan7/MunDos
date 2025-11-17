using UnityEngine;

public class DebugToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] showIfDebugMode;
    [SerializeField] private GameObject[] showIfNotDebugMode;

    private void Update()
    {
        if (showIfDebugMode.Length > 0 && SettingsInfo.debugMode) foreach (GameObject obj in showIfDebugMode) obj.SetActive(true);
        else foreach (GameObject obj in showIfDebugMode) obj.SetActive(false);
        if (showIfNotDebugMode.Length > 0 && !SettingsInfo.debugMode) foreach (GameObject obj in showIfNotDebugMode) obj.SetActive(true);
        else foreach (GameObject obj in showIfNotDebugMode) obj.SetActive(false);
    }
}
