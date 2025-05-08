using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to your Projectile prefab.
public class ProjectileCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy the enemy
        } else if (collision.gameObject.CompareTag("Player"))
        {
            // Find the player's PlayerShoot component and add the projectile back.
            PlayerShoot playerShoot = collision.gameObject.GetComponent<PlayerShoot>();
            if (playerShoot != null)
            {
                playerShoot.RecoverProjectile();
            }
            
            // Destroy this projectile.
            Destroy(gameObject);
        }
    }
}
