using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;
    public float dashDistance = 7f;
    public float dashSpeed = 24f; // Updated to match player's dash speed
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.25f; // Updated to match player's dash cooldown
    public float jumpForce = 10f; // Updated to match player's jump force
    public float jumpGravityMultiplier = 2f; // Add this variable
    public float fallGravityMultiplier = 5f; // Add this variable
    public LayerMask groundLayer;
    public float raycastDistance = 0.5f;
    public Transform groundCheck;
    public Transform frontCheck;
    public float stunDuration = 2f; // Variable for stun duration
    public float HitDuractionTime = 1.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private bool isStunned = false; // Variable to track stun state
    private float lastDashTime;
    private bool facingRight = true;
    private int dashCount = 0; // Add a dash counter

    [SerializeField] private TrailRenderer tr; // Add a TrailRenderer component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f; // Ensure gravity scale is set correctly
        if (player == null && GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (player == null || isStunned) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Flip sprite based on player position
        if ((player.position.x > transform.position.x && !facingRight) || 
            (player.position.x < transform.position.x && facingRight))
        {
            Flip();
        }

        // Reset dash count after cooldown
        if (Time.time > lastDashTime + dashCooldown)
        {
            dashCount = 0;
        }

        // Check if we should dash
        if ((distanceToPlayer <= dashDistance && distanceToPlayer > detectionRadius) && dashCount < 2)
        {
            StartCoroutine(Dash());
        }
        
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Wall and edge detection
        bool hitWall = Physics2D.Raycast(frontCheck.position, facingRight ? Vector2.right : Vector2.left, raycastDistance, groundLayer);
        bool edgeAhead = !Physics2D.Raycast(frontCheck.position, Vector2.down, raycastDistance * 2, groundLayer);
        bool platformAhead = Physics2D.Raycast(frontCheck.position, facingRight ? Vector2.right : Vector2.left, raycastDistance * 2, groundLayer);

        if ((hitWall || edgeAhead) && isGrounded && platformAhead)
        {
            Jump();
        }

        // Apply gravity scale for jumping and falling
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
    }

    void FixedUpdate()
    {
        if (player == null || isDashing || isStunned) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Increase the dash count
        dashCount++;

        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;

        tr.emitting = true; // Enable trail rendering
        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false; // Disable trail rendering

        isDashing = false;
        rb.gravityScale = originalGravity;

        // If we've dashed twice, start cooldown
        if (dashCount >= 2)
        {
            lastDashTime = Time.time;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dashDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(Stun());
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HitDuraction());
        }
    }

    IEnumerator Stun()
    {
        isStunned = true;
        rb.velocity = Vector2.zero; // Stop movement
        // Optional: Add visual feedback for stun state here

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    IEnumerator HitDuraction()
    {
        isStunned = true;
        rb.velocity = Vector2.zero; // Stop movement
        
        yield return new WaitForSeconds(HitDuractionTime);

        isStunned = false;
    }
}
