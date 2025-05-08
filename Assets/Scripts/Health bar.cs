using UnityEngine;
using UnityEngine.SceneManagement; // Needed to reload the scene

public class PlayerGameOver : MonoBehaviour
{
    private int hitCount = 0;
    private int maxHits = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy")) // Make sure the enemy has the tag "Enemy"
        {
            hitCount++;
            Debug.Log("Player hit count: " + hitCount + "/" + maxHits);

            if (hitCount >= maxHits)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(3);
    }
}
