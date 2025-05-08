using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; // Empty GameObject for shooting direction
    public float fireRate = 1.5f;
    public float projectileSpeed = 10f;
    public float detectionRadius = 5f; // Detection range for the player

    private Transform player;
    private Rigidbody2D rb;
    private float nextFireTime;
    private bool isFalling = false;
    private bool playerInRange = false; // Flag to check if player is in range

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Keeps turret on platform initially
    }

    void Update()
    {
        if (player != null)
        {
            DetectPlayer();

            if (playerInRange)
            {
                TrackPlayer();
                ShootAtPlayer();
            }
        }
    }

    void DetectPlayer()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within the detection radius, set playerInRange to true
        if (distanceToPlayer <= detectionRadius)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    void TrackPlayer()
    {
        // Aim the firePoint towards the player
        Vector3 direction = player.position - firePoint.position; // Aim from firePoint
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle); // Rotate only firePoint
    }

    void ShootAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * projectileSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Ball")) // Hit by player's projectile
        {
            isFalling = true;
            rb.bodyType = RigidbodyType2D.Dynamic; // Enables gravity
            rb.velocity = new Vector2(0, -2f); // Optional: Slight push downward
            transform.SetParent(null); // Detach from platform
        }
    }
}
