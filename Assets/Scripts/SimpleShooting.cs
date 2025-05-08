using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile to shoot
    public Transform shootingPoint;     // The point from which the projectile will be fired
    public float projectileSpeed = 10f; // Speed of the projectile
    public float fireRate = 0.5f;      // How often the player can shoot (in seconds)
    
    private float nextFireTime = 0f;

    private void Update()
    {
        // Shooting mechanic (fire when space key is pressed and cooldown is over)
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Set the next time the player can shoot
        }
    }

    // Method to handle shooting
    private void Shoot()
    {
        // Instantiate the projectile at the shooting point
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        
        // Get the Rigidbody2D component of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
        // If the Rigidbody2D exists, apply a force to make the projectile move
        if (rb != null)
        {
            rb.velocity = transform.right * projectileSpeed; // Move the projectile forward
        }
    }
}
