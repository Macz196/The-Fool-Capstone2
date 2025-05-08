using System.Collections;
using UnityEngine;

public class EnemyJumpAI : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius to detect projectiles
    public float jumpForce = 10f; // Jump force
    public float jumpCooldown = 2f; // Cooldown duration
    public LayerMask projectileLayer; // Layer for projectiles (Projectile layer)
    private Rigidbody2D rb;
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DetectProjectile();
    }

    void DetectProjectile()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, projectileLayer);
        if (hits.Length > 0 && canJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false;
        StartCoroutine(ResetJumpCooldown());
    }

    IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}