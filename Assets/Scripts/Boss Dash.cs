using UnityEngine;

public class BossDash : MonoBehaviour
{
    public float dashSpeed = 10f; // Speed of the dash
    private Vector2 dashDirection = Vector2.right; // Initial direction
    private Rigidbody2D rb;
    public LayerMask playerLayer; // Layer mask for the player
    public float speedBoost = 15f; // Speed when detecting player
    public float boostDuration = 2f; // Duration of speed boost
    public float boostCooldown = 3f; // Cooldown before next boost

    private bool isBoosted = false;
    private float normalSpeed;
    private bool canBoost = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dashDirection * dashSpeed; // Start moving immediately
        normalSpeed = dashSpeed; // Store original speed
    }

    void FixedUpdate()
    {
        rb.velocity = dashDirection * dashSpeed; // Keep moving in the current direction
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Wall detection
        {
            ReverseDirection();
        }
        else if (IsPlayer(collision)) // Check if collision is with the player
        {
            // Handle collision with player without stopping or affecting the dash
        }
    }

    void ReverseDirection()
    {
        dashDirection *= -1; // Flip direction
        rb.velocity = dashDirection * dashSpeed; // Immediately apply new velocity
    }

    private bool IsPlayer(Collision2D collision)
    {
        return (playerLayer == (playerLayer | (1 << collision.gameObject.layer)));
    }

    // Detect when the player enters the detection area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && canBoost)
        {
            StartCoroutine(SpeedBoost());
        }
    }

    private System.Collections.IEnumerator SpeedBoost()
    {
        isBoosted = true;
        dashSpeed = speedBoost;
        yield return new WaitForSeconds(boostDuration);

        // Reset to normal speed and start cooldown
        dashSpeed = normalSpeed;
        isBoosted = false;
        canBoost = false;

        yield return new WaitForSeconds(boostCooldown);
        canBoost = true;
    }
}
