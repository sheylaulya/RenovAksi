using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 12f;
    public float groundCheckRadius = 0.2f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool facingRight = true;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        float move = 0f;

        // === INPUT LEFT ===
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (InputManager.Instance == null || InputManager.Instance.IsAllowed(InputType.MoveLeft))
            {
                move = -1;

                if (TutorialManager.Instance != null)
                    TutorialManager.Instance.ReportInput(InputType.MoveLeft);
            }
        }

        // === INPUT RIGHT ===
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (InputManager.Instance == null || InputManager.Instance.IsAllowed(InputType.MoveRight))
            {
                move = 1;

                if (TutorialManager.Instance != null)
                    TutorialManager.Instance.ReportInput(InputType.MoveRight);
            }
        }

        moveInput = move;

        // === GROUND CHECK ===
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // === RUN LOGIC ===
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        bool isRunning = runPressed && moveInput != 0 &&
                         (InputManager.Instance == null || InputManager.Instance.IsAllowed(InputType.Run));

        if (runPressed && (InputManager.Instance == null || InputManager.Instance.IsAllowed(InputType.Run)))
        {
            if (TutorialManager.Instance != null)
                TutorialManager.Instance.ReportInput(InputType.Run);
        }

        if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        // === ANIMATION ===
        float speedValue = Mathf.Abs(moveInput);

        if (isRunning)
            speedValue *= 2f;

        animator.SetFloat("speed", speedValue);

        // === FLIP CHARACTER ===
        if (facingRight && moveInput < 0)
            Flip();
        else if (!facingRight && moveInput > 0)
            Flip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}