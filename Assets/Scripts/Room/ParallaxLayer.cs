using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float originalPositionX;
    [SerializeField] private float originalPositionY;

    private void OnEnable()
    {
        ResetLocation();
    }

    private void OnDisable()
    {
        transform.localPosition = new Vector2(originalPositionX, originalPositionY);
    }

    public void ResetLocation()
    {
        transform.localPosition = new Vector2(originalPositionX, originalPositionY);
    }

    public float parallaxFactorX;
    public float parallaxFactorY;

    public void Move(Vector2 delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x += delta.x * parallaxFactorX;
        newPos.y += delta.y * parallaxFactorY;

        transform.localPosition = newPos;
    }

}