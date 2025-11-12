using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera parallaxCamera;

    [Tooltip("This is the virtual camera GameObject that represents the room.")]
    public GameObject targetVirtualCamObject;

    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    void Start()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        SetLayers();
    }

    void SetLayers()
    {
        parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();
            if (layer != null) parallaxLayers.Add(layer);
        }
    }

    void Move(Vector2 delta)
    {
        if (targetVirtualCamObject != null && !targetVirtualCamObject.activeInHierarchy)
        {
            return;
        }

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
