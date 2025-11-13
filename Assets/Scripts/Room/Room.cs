using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using NaughtyAttributes;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject roomCamera;
    [SerializeField] public AudioSource connectedAudioSource; // Used in BGMsManager.cs
    [SerializeField] private BGMsManager bgmsManager;
    [SerializeField] private GameObject[] parallaxObjects;
    [SerializeField] private bool showFirst;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = col.gameObject.transform;

        roomCamera.SetActive(true);
        if (bgmsManager != null && connectedAudioSource != null) bgmsManager.ChangeBGMWrapper(gameObject, true);

        if (parallaxObjects.Length > 0) foreach (GameObject parallaxObject in parallaxObjects)
        {
                if (!showFirst) parallaxObject.SetActive(false);
                else showFirst = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Protag") return;

        roomCamera.SetActive(false);
        if (bgmsManager != null && connectedAudioSource != null) bgmsManager.ChangeBGMWrapper(gameObject, false);
    }
}
