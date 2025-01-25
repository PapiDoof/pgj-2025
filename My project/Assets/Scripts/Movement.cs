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

    private float horizontalInput;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump when grounded and jump button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
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
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Set jump velocity
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
