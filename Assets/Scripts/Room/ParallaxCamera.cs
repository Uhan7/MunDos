using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector3 oldPosition;

    void Start()
    {
        oldPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        Vector2 delta = new Vector2(
            oldPosition.x - newPosition.x,
            oldPosition.y - newPosition.y
        );

        if (delta != Vector2.zero && onCameraTranslate != null)
        {
            onCameraTranslate(delta);
        }

        oldPosition = newPosition;
    }
}
