using UnityEngine;

public class Friction : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;

    // Variables
    [SerializeField] private float floorFriction = 0.9f;
    [SerializeField] private float airResistance = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyFriction();
    }

    // FixedUpdate Functions ---------------------------------------------------

    void ApplyFriction()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX * floorFriction, rb.linearVelocityY / airResistance);
    }
}
