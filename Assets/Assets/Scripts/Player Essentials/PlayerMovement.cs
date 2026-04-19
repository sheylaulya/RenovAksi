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
        // Input horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // === RUN / WALK LOGIC ===
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && moveInput != 0;

        if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        // === ANIMATION (SINGLE PARAMETER) ===
        float speedValue = Mathf.Abs(moveInput);

        if (isRunning)
            speedValue *= 2f; // supaya beda antara jalan & lari

        animator.SetFloat("speed", speedValue);

        // === FLIP CHARACTER ===
        if (facingRight && moveInput < 0)
            Flip();
        else if (!facingRight && moveInput > 0)
            Flip();
    }

    void FixedUpdate()
    {
        // Apply movement
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