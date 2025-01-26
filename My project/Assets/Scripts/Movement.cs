using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float maxSpeed = 7f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float jumpDeceleration = 2f; // Vertical deceleration when jumping upwards

    [Header("Ground Detection")]
    public LayerMask groundLayer; // Layer for tilemap or ground
    private bool isGrounded;

    [Header("Falling Settings")]
    public float fallAcceleration = 5f; // Additional downward acceleration when falling

    public Animator animator;
    private float horizontalInput;
    private Rigidbody2D rb;

    private bool wasGrounded; // Tracks if the player was previously grounded
    private Vector3 defaultScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultScale = transform.localScale; // Store the default scale for flipping
    }

    private void Update()
    {
        // Get horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump when grounded and jump button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            animator.SetBool("isJump", true);
        }

        // Update the Speed parameter in Animator
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Handle character rotation based on movement direction
        if (horizontalInput > 0) // Moving right
        {
            transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
        }
        else if (horizontalInput < 0) // Moving left
        {
            transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
        }
    }

    private void FixedUpdate()
    {
        // Smooth horizontal movement
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.linearVelocity.x;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, 0.9f) * Mathf.Sign(speedDifference);
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        // Limit maximum speed
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
        }

        // Add downward acceleration when falling
        if (rb.linearVelocity.y < 0) // If the player is falling
        {
            rb.gravityScale = 1; // Use default gravity scale
            rb.linearVelocity += new Vector2(0, fallAcceleration * Time.fixedDeltaTime); // Increase downward speed
        }
        else if (rb.linearVelocity.y > 0) // If the player is jumping upwards
        {
            // Apply upward deceleration for a more controlled jump
            rb.linearVelocity += new Vector2(0, -jumpDeceleration * Time.fixedDeltaTime); // Slow down upwards movement
        }

        // Handle landing detection
        if (isGrounded && !wasGrounded)
        {
            OnLanding();
        }

        wasGrounded = isGrounded; // Update grounded state
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Set jump velocity
    }

    private void OnLanding()
    {
        animator.SetBool("isJump", false); // Transition to idle/running animation
       // Debug.Log("Player landed!");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if we are touching the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if we are no longer touching the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
