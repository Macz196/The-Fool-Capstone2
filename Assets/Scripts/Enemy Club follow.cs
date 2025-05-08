using UnityEngine;
using System.Collections;

/*public class EnemyFollowAndStun : MonoBehaviour
{
    public Transform targetPlayer;
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;
    public float jumpForce = 7f; // Increased force for more noticeable jump
    public float knockbackForce = 5f; // Force applied to knock back player

    private Rigidbody2D rb;
    private bool isStunned = false;
    private bool facingRight = true; // Track enemy facing direction

    // Reference to the slamHitbox (child of the club)
    public Transform slamHitbox;

    // Delay before activating the ground slam (in seconds)
    public float slamDelay = 0.5f;

    // Used to track when the player enters the hitbox area
    private bool isPlayerInHitbox = false;

    // Cooldown time for the ground slam (in seconds)
    public float slamCooldown = 3f;
    private bool isSlamOnCooldown = false;

    // Sleep duration after slam
    public float sleepDuration = 1f;
    private bool isSleeping = false; // Flag to indicate the enemy is sleeping
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (targetPlayer == null && GameObject.FindGameObjectWithTag("Player"))
        {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (slamHitbox == null)
        {
            slamHitbox = transform.Find("slamHitbox"); // Assuming slamHitbox is a child of the enemy (club)
        }
    }

    void Update()
    {
        if (targetPlayer == null || isStunned || isSleeping) return;

        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

        // Flip enemy to face the player and flip the hitbox
        FlipTowardsPlayer();

        // Check if the player is inside the slamHitbox and slam is off cooldown
        if (slamHitbox != null && slamHitbox.GetComponent<Collider2D>().IsTouching(targetPlayer.GetComponent<Collider2D>()))
        {
            if (!isPlayerInHitbox && !isSlamOnCooldown)
            {
                isPlayerInHitbox = true;
                StartCoroutine(PerformJumpAndSlam()); // Start jump before slam
            }
        }
        else
        {
            isPlayerInHitbox = false;
        }

        // If player is in the detection radius and not stunned, move toward the player
        if (distanceToPlayer <= detectionRadius && !isStunned && !isPlayerInHitbox)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement if player is out of detection radius or in hitbox
        }
    }

    // Coroutine to handle jump before slam
    IEnumerator PerformJumpAndSlam()
    {
        Debug.Log("Enemy jumping before slam!");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Wait until the enemy starts falling
        yield return new WaitUntil(() => rb.velocity.y <= 0);

        ActivateGroundSlam();
    }

    // Method to activate the ground slam
    void ActivateGroundSlam()
    {
        Debug.Log("Ground Slam Activated!");
        DealDamageAndKnockback();
        StartCoroutine(SlamCooldown());
        StartCoroutine(SleepAfterSlam());
    }

    // Method to deal damage and knock back the player
    void DealDamageAndKnockback()
    {
        if (targetPlayer != null)
        {
            // Check if the player has the 'PlayerGameOver' script
            PlayerGameOver playerHealth = targetPlayer.GetComponent<PlayerGameOver>();
            if (playerHealth != null)
            {
                playerHealth.OnPlayerHit(); // Simulate damage
                Debug.Log("Player hit by ground slam.");
            }
            
            // Apply knockback force to the player
            Rigidbody2D playerRb = targetPlayer.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (targetPlayer.position - transform.position).normalized;
                playerRb.velocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackForce * 0.5f); // Apply more horizontal force
            }
        }
    }

    // Coroutine to handle the cooldown for the ground slam
    IEnumerator SlamCooldown()
    {
        isSlamOnCooldown = true;
        Debug.Log("Slam is on cooldown!");
        yield return new WaitForSeconds(slamCooldown);
        isSlamOnCooldown = false;
        Debug.Log("Slam cooldown finished!");
    }

    // Coroutine to handle the sleep state after the slam
    IEnumerator SleepAfterSlam()
    {
        isSleeping = true;  // Set enemy to sleep state
        Debug.Log("Enemy is sleeping!");
        yield return new WaitForSeconds(sleepDuration);
        isSleeping = false;  // Wake the enemy up after sleep duration is over
        Debug.Log("Enemy woke up!");
    }

    // Method to flip the enemy and its slam hitbox to face the player
    void FlipTowardsPlayer()
    {
        if (targetPlayer != null)
        {
            if ((targetPlayer.position.x < transform.position.x && facingRight) || (targetPlayer.position.x > transform.position.x && !facingRight))
            {
                facingRight = !facingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1; // Flip the enemy
                transform.localScale = scale;

                // Flip the slam hitbox
                if (slamHitbox != null)
                {
                    Vector3 hitboxScale = slamHitbox.localScale;
                    hitboxScale.x *= -1;
                    slamHitbox.localScale = hitboxScale;
                }
            }
        }
    }
}*/