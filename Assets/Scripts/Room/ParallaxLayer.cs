using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float positionX;
    [SerializeField] private float positionY;

    private void OnEnable()
    {
        
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