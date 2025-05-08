using System; // Needed for Action
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerJump; // Event for detecting jumps

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpGravityMultiplier = 2f;
    public float fallGravityMultiplier = 5f;
    public float knockbackForceX = 10f; // Horizontal knockback force
    public float knockbackForceY = 5f;  // Vertical knockback force
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.25f;
    private float horizontalDirection;
    private int dashCount = 0;
    private int maxDashCount = 2;
    public GameObject MovementPanel;
    private bool MovementPanelOpen = false;

    [SerializeField] private TrailRenderer tr;

    private bool isKnockback = false; // Flag to indicate knockback state

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
    }

    void Update()
    {
        if (isDashing || isKnockback)
        {
            return; // Skip movement input during dash or knockback
        }

        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Track the direction the player is facing
        if (moveInput != 0)
        {
            horizontalDirection = Mathf.Sign(moveInput);
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            OnPlayerJump?.Invoke(); // Broadcast jump event
        }

        // Apply different gravity scales depending on the player's vertical velocity
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = jumpGravityMultiplier;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        // Handle dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Handle movement panel toggling
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MovementPanelOpen = !MovementPanelOpen;
            MovementPanel.SetActive(MovementPanelOpen);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            dashCount = 0;
            isKnockback = false; // Reset knockback when grounded
        }
        else if (collision.gameObject.CompareTag("Enemy")) // Check for enemy collision
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            // Apply knockback force
            rb.AddForce(new Vector2(knockbackDirection.x * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);

            isKnockback = true; // Set the knockback state

            StartCoroutine(ResetKnockback()); // Reset knockback after a delay
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Check if dashing up or horizontally
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, dashingPower - 12f);
        }
        else
        {
            rb.velocity = new Vector2(horizontalDirection * dashingPower, 0f);
        }

        tr.emitting = true;
        dashCount++;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        if (dashCount >= maxDashCount)
        {
            dashCount = 0;
            yield return new WaitForSeconds(dashingCooldown);
        }

        canDash = true;
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the duration as needed
        isKnockback = false;
    }
}
