using UnityEngine;
using System.Collections;

public class DaggerEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;
    public float dashSpeed = 24f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1.5f;
    public float stopTime = 1.5f; // Time to wait before attacking again
    public float gravityScale = 2f; // Normal gravity scale
    public float raycastDistance = 0.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform frontCheck;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool isStopping = false;
    private bool isGrounded = false;
    private float lastDashTime;
    private Vector2 dashDirection;
    private bool facingRight = true;

    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Ensure dagger has gravity
        if (player == null && GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null || isDashing || isStopping) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Flip sprite based on player position
        if ((player.position.x > transform.position.x && !facingRight) ||
            (player.position.x < transform.position.x && facingRight))
        {
            Flip();
        }

        // Dash when player is in detection range
        if (distanceToPlayer <= detectionRadius && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing || isStopping) return;

        // Check if dagger hits a wall
        bool hitWall = Physics2D.Raycast(frontCheck.position, facingRight ? Vector2.right : Vector2.left, raycastDistance, groundLayer);

        if (hitWall)
        {
            StopDash(); // Stop immediately if hitting a wall
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rb.gravityScale = 0f; // Disable gravity while dashing
        rb.velocity = Vector2.zero; // Reset velocity
        dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;

        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;

        StopDash();
    }

    void StopDash()
    {
        isDashing = false;
        isStopping = true;
        rb.velocity = Vector2.zero; // Stop movement
        rb.gravityScale = gravityScale; // Restore gravity

        Invoke(nameof(TurnAround), stopTime); // Wait before turning
    }

    void TurnAround()
    {
        isStopping = false;
        Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

