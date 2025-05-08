using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public Image healthBar; // Assign the health bar fill image
    public GameObject healthBarUI; // Assign the full health bar UI (for hiding/showing)
    public Collider2D healthBarArea; // Assign an Empty GameObject with a BoxCollider2D (Trigger)

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        healthBarUI.SetActive(false); // Hide health bar initially
    }

    private void Update()
    {
        // Check if player is in the assigned health bar area
        if (healthBarArea != null)
        {
            Collider2D player = Physics2D.OverlapBox(healthBarArea.bounds.center, healthBarArea.bounds.size, 0, LayerMask.GetMask("Player"));
            healthBarUI.SetActive(player != null); // Show if player is inside
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!");
        healthBarUI.SetActive(false); // Hide health bar when boss dies
        gameObject.SetActive(false); // Disable boss
    }
}
