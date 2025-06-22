using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Variables ---------------------------------------------------------------

    // Components
    [HideInInspector] public Rigidbody2D rb; // Used in PlayerAnimator

    // References
    [SerializeField] private GameObject floorChecker;
    private FloorChecker floorCheckerScript;

    // Inputs
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode walkKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    // Horizontal Movement
    [SerializeField] private float runSpeed = 55f;
    [SerializeField] private float maxRunSpeed = 9.5f;

    [SerializeField] private float walkSpeed = 20f;
    [SerializeField] private float maxWalkSpeed = 4.5f;

    // Vertical Movement
    [SerializeField] private float jumpForce = 10f;

    // Flags
    private bool getMoveLeftKey;
    private bool getMoveRightKey;
    private bool isWalking;
    private bool jumpIsQueued;

    // Functions ---------------------------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        floorCheckerScript = floorChecker.GetComponent<FloorChecker>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        InputUpdate();
        AnimUpdate();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        Jump();

        LimitVelocity();
    }

    // Update Functions --------------------------------------------------------

    void InputUpdate()
    {
        getMoveLeftKey = Input.GetKey(moveLeftKey);
        getMoveRightKey = Input.GetKey(moveRightKey);
        isWalking = Input.GetKey(walkKey);
        if (Input.GetKeyDown(jumpKey) && floorCheckerScript.onGround) jumpIsQueued = true;
    }

    void AnimUpdate()
    {
        // Hello future johann, change this to actually flip it using sprite renderer

        //if (getMoveLeftKey) transform.localScale = new Vector3(-1, 1, 1);
        //if (getMoveRightKey) transform.localScale = new Vector3(1, 1, 1);
    }

    // FixedUpdate Functions ---------------------------------------------------

    void HorizontalMovement()
    {
        float currentSpeed = isWalking ? walkSpeed : runSpeed;

        if (getMoveLeftKey) rb.AddForce(Vector2.left * currentSpeed, ForceMode2D.Force);
        if (getMoveRightKey) rb.AddForce(Vector2.right * currentSpeed, ForceMode2D.Force);
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
        float maxSpeed = isWalking ? maxWalkSpeed : maxRunSpeed;

        if (rb.linearVelocityX > maxSpeed) rb.linearVelocity = new Vector2(maxSpeed, rb.linearVelocityY);
        if (rb.linearVelocityX < -maxSpeed) rb.linearVelocity = new Vector2(-maxSpeed, rb.linearVelocityY);
    }
}
