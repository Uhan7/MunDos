using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;

    void Start()
    {
        AdjustCameraViewport();
    }

    void AdjustCameraViewport()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        float currentAspectRatio = (float)Screen.width / Screen.height;

        if (currentAspectRatio > targetAspectRatio)
        {
            float scaleWidth = targetAspectRatio / currentAspectRatio;
            cam.rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
        }
        else if (currentAspectRatio < targetAspectRatio)
        {
            float scaleHeight = currentAspectRatio / targetAspectRatio;
            cam.rect = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else
        {
            cam.rect = new Rect(0f, 0f, 1f, 1f);
        }

        cam.backgroundColor = Color.black;
    }
}