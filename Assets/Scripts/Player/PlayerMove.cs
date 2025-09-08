using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Variables ---------------------------------------------------------------

    [Header("Components")]
    [HideInInspector] public Rigidbody2D rb; // Used in Animator
    [HideInInspector] private Animator anim;

    [Header("References")]
    //[SerializeField] private GameObject floorChecker;
    //[HideInInspector] private FloorChecker floorCheckerScript;

    [Header("Inputs")]
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    //[SerializeField] private KeyCode walkKey = KeyCode.LeftShift;
    //[SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Horizontal Movement")]
    [SerializeField] private float runSpeed = 55f;
    [SerializeField] private float maxRunSpeed = 9.5f;
    //[SerializeField] private float walkSpeed = 20f;
    //[SerializeField] private float maxWalkSpeed = 4.5f;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Flags")]
    [HideInInspector] public bool canMove = true; // Used in DialogueTrigger.cs
    [HideInInspector] private bool getMoveLeftKey;
    [HideInInspector] private bool getMoveRightKey;
    [HideInInspector] private bool isWalking;
    [HideInInspector] private bool jumpIsQueued;

    // Functions ---------------------------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Animator>() != null) anim = GetComponent<Animator>();

        //floorCheckerScript = floorChecker.GetComponent<FloorChecker>();
    }

    private void Start()
    {
        canMove = true;
    }

    void Update()
    {
        InputUpdate();
        AnimUpdate();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        //Jump();

        LimitVelocity();
    }

    // Update Functions --------------------------------------------------------

    void InputUpdate()
    {
        if (!canMove)
        {
            getMoveLeftKey = false;
            getMoveRightKey = false;
            //isWalking = false;
            //jumpIsQueued = false;

            return;
        }

        getMoveLeftKey = Input.GetKey(moveLeftKey);
        getMoveRightKey = Input.GetKey(moveRightKey);
        //isWalking = Input.GetKey(walkKey);

        //if (Input.GetKeyDown(jumpKey) && floorCheckerScript.onGround) jumpIsQueued = true;
    }

    void AnimUpdate()
    {
        if (anim != null) anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
        // TEMP
        if (getMoveLeftKey) GetComponent<SpriteRenderer>().flipX = true;
        if (getMoveRightKey) GetComponent<SpriteRenderer>().flipX = false;
        // TEMP
    }

    // FixedUpdate Functions ---------------------------------------------------

    void HorizontalMovement()
    {
        /*
        float currentSpeed = isWalking ? walkSpeed : runSpeed;

        if (getMoveLeftKey) rb.AddForce(Vector2.left * currentSpeed, ForceMode2D.Force);
        if (getMoveRightKey) rb.AddForce(Vector2.right * currentSpeed, ForceMode2D.Force);
        */

        if (getMoveLeftKey) rb.AddForce(Vector2.left * runSpeed, ForceMode2D.Force);
        if (getMoveRightKey) rb.AddForce(Vector2.right * runSpeed, ForceMode2D.Force);
    }

    /*
    void Jump()
    {
        if (jumpIsQueued)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpIsQueued = false;
        }
    }
    */

    void LimitVelocity()
    {
        /*
        float maxSpeed = isWalking ? maxWalkSpeed : maxRunSpeed;

        if (rb.linearVelocityX > maxSpeed) rb.linearVelocity = new Vector2(maxSpeed, rb.linearVelocityY);
        if (rb.linearVelocityX < -maxSpeed) rb.linearVelocity = new Vector2(-maxSpeed, rb.linearVelocityY);
        */

        if (rb.linearVelocityX > maxRunSpeed) rb.linearVelocity = new Vector2(maxRunSpeed, rb.linearVelocityY);
        if (rb.linearVelocityX < -maxRunSpeed) rb.linearVelocity = new Vector2(-maxRunSpeed, rb.linearVelocityY);
    }
}
