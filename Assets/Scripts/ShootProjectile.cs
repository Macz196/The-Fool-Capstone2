using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab1;       // Assign your first projectile prefab in the inspector
    public GameObject projectilePrefab2;       // Assign your second projectile prefab in the inspector
    private GameObject currentProjectilePrefab; // Current selected projectile prefab
    
    public Transform shootPoint;              // Assign the shooting point in the inspector
    public float projectileSpeed = 10f;         // Speed of the projectile
    private bool isFacingRight = true;          // Assume the player starts facing right
    public bool canShoot = true;
    public int maxProjectiles = 4;              // Maximum number of projectiles available
    private int currentProjectiles;             // Current available projectile count
    public TextMeshProUGUI projectileCountText; // Reference to a TextMeshProUGUI element in the scene
    public TextMeshProUGUI projectileTypeText;  // Reference to display projectile type

    private Rigidbody2D rb;                   // The player's Rigidbody2D
    private bool usingFirstProjectile = true;  // Track which projectile is currently selected

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentProjectiles = maxProjectiles;
        currentProjectilePrefab = projectilePrefab1;
        UpdateUI();
    }

    void Update()
    {
        // Update the player's direction and flip if needed.
        if (Input.GetKey(KeyCode.A))
        {
            if (isFacingRight)
            {
                Flip();
            }
            isFacingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isFacingRight)
            {
                Flip();
            }
            isFacingRight = true;
        }

        // Switch projectile type when pressing R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchProjectileType();
        }

        // Shoot when the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0) && canShoot && currentProjectiles > 0 && Time.timeScale > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile.
        GameObject projectile = Instantiate(currentProjectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            // Base horizontal direction.
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

            // Adjust the shot direction if the player is moving vertically.
            if (Mathf.Abs(rb.velocity.y) > 0.1f)
            {
                direction += rb.velocity.y > 0 ? Vector2.up : Vector2.down;
            }

            direction.Normalize();
            projectileRb.velocity = direction * projectileSpeed;
        }
        StartCoroutine(ShootDelay());
        currentProjectiles--;
        UpdateUI();
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.35f);
        canShoot = true;
    }

    void Flip()
    {
        // Flip the player's local scale on the X axis.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SwitchProjectileType()
    {
        usingFirstProjectile = !usingFirstProjectile;
        currentProjectilePrefab = usingFirstProjectile ? projectilePrefab1 : projectilePrefab2;
        UpdateUI();
    }

    // Called by the Projectile script when a projectile collides with the player.
    public void RecoverProjectile()
    {
        currentProjectiles = Mathf.Min(currentProjectiles + 1, maxProjectiles);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (projectileCountText != null)
        {
            projectileCountText.text = "Balls: " + currentProjectiles + " / " + maxProjectiles;
        }
        if (projectileTypeText != null)
        {
            projectileTypeText.text = "Projectile: " + (usingFirstProjectile ? "Type 1" : "Type 2");
        }
    }
}