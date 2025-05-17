using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;

    // Inputs
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    // Horizontal Movement
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float frictionX = 0.9f;
    [SerializeField] private float frictionY = 1f;

    // Vertical Movement
    [SerializeField] private float jumpForce = 10f;

    // Flags
    private bool getMoveLeftKey;
    private bool getMoveRightKey;
    private bool jumpIsQueued;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputUpdate();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        Jump();

        LimitVelocity();
        ApplyFriction();
    }

    // Update Functions --------------------------------------------------------

    void InputUpdate()
    {
        getMoveLeftKey = Input.GetKey(moveLeftKey);
        getMoveRightKey = Input.GetKey(moveRightKey);
        if (Input.GetKeyDown(jumpKey)) jumpIsQueued = true;
    }

    // FixedUpdate Functions ---------------------------------------------------

    void HorizontalMovement()
    {
        if (getMoveLeftKey) rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Force);
        if (getMoveRightKey) rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
    }

    void Jump()
    {
        if (jumpIsQueued)
        {
            print("jump");
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpIsQueued = false;
        }
    }

    void LimitVelocity()
    {
        if (rb.linearVelocityX > maxSpeed) rb.linearVelocity = new Vector2(maxSpeed, rb.linearVelocityY);
        if (rb.linearVelocityX < -maxSpeed) rb.linearVelocity = new Vector2(-maxSpeed, rb.linearVelocityY);
    }

    void ApplyFriction()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX * frictionX, rb.linearVelocityY * frictionY);
    }

}
