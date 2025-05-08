using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHits = 3; // Number of collisions before destruction
    private int currentHits = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            currentHits++;
            
            if (currentHits >= maxHits)
            {
                Destroy(gameObject); // Destroy the enemy
            }
        }
    }
}