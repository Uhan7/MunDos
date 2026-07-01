using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ZoomEnviManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject zoomEnviBackdrop;
    [SerializeField] private GameObject zoomEnviImage;

    [Header("Flags")]
    [HideInInspector] public bool activated = false; // Used in DialogueTrigger.cs (to sync with Focus)

    public UnityEvent CallZoomEvniSubComponents;
    private void Update()
    {
        //if (Input.GetKeyDown(deactivateKey)) DeactivateZoomedEnvi();
    }

    // Helper Functions --------------------------------------------------------

    public void ActivateZoomedEnvi() // used in InteractableObject.cs
    {
        if (zoomEnviBackdrop != null) zoomEnviBackdrop.SetActive(true);
        zoomEnviImage.SetActive(true);

        activated = true;
        //Debug.Log("Invoking");
        CallZoomEvniSubComponents.Invoke();
    }

    // I put this as a separate function since it might be executed differently soon
    public void DeactivateZoomedEnvi() // used in InteractableObject.cs
    {
        //print("deactivate zoom called");
        if (zoomEnviBackdrop != null) zoomEnviBackdrop.SetActive(false);
        zoomEnviImage.SetActive(false);

        activated = false;
    }
}
