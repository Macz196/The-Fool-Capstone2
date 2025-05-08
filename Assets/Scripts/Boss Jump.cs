using System.Collections;
using UnityEngine;

public class BossJumpReaction : MonoBehaviour
{
    public float jumpForce = 7f; // Adjust jump height
    public float jumpChance = 0.5f; // 50% chance to jump
    private Rigidbody2D rb;
    private bool isGrounded = true; // Track if the boss is grounded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerController.OnPlayerJump += TryJump; // Subscribe to the event
    }

    void OnDestroy()
    {
        PlayerController.OnPlayerJump -= TryJump; // Unsubscribe to prevent memory leaks
    }

    void TryJump()
    {
        if (!isGrounded) return; // Prevent jumping in mid-air

        Debug.Log("Player Jumped! Boss deciding...");

        if (Random.value < jumpChance) // Random chance to jump
        {
            Debug.Log("Boss is jumping!");
            Jump();
            isGrounded = false; // Prevent jumping until grounded
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Allow jumping again when boss touches the ground
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

}

