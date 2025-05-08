using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableItem
{
    public GameObject prefab; // The prefab to spawn
    public int quantity; // The amount to spawn
}

public class Spawner : MonoBehaviour
{
    public List<SpawnableItem> spawnableItems; // Assign items in the Inspector
    public Transform spawnPoint; // Set spawn location (default is the spawner's position)
    public float interactionRange = 2f; // Distance within which the player can activate
    private bool hasSpawned = false; // Ensures it spawns only once
    public float spawnDelay = 3f; // Delay before spawning
    public float freezeDuration = 3f; // Duration objects stay frozen

    void Update()
    {
        if (!hasSpawned && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector2.Distance(transform.position, player.transform.position) <= interactionRange)
            {
                StartCoroutine(SpawnObjectsWithDelay());
            }
        }
    }

    IEnumerator SpawnObjectsWithDelay()
    {
        hasSpawned = true; // Prevent multiple activations
        yield return new WaitForSeconds(spawnDelay); // Wait before spawning

        if (spawnableItems.Count == 0) yield break;

        List<SpawnableItem> selectedItems = new List<SpawnableItem>();

        // Randomly select 2 different objects
        while (selectedItems.Count < 2)
        {
            SpawnableItem randomItem = spawnableItems[Random.Range(0, spawnableItems.Count)];
            if (!selectedItems.Contains(randomItem))
            {
                selectedItems.Add(randomItem);
            }
        }

        foreach (SpawnableItem item in selectedItems)
        {
            for (int i = 0; i < item.quantity; i++) // Spawn specified quantity
            {
                GameObject spawnedObj = Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
                StartCoroutine(FreezeObject(spawnedObj)); // Apply freeze effect
                DisplayQuantity(spawnedObj, item.quantity);
            }
        }
    }

    IEnumerator FreezeObject(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        Collider2D col = obj.GetComponent<Collider2D>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze movement
        }

        if (col != null)
        {
            col.enabled = false; // Disable interactions
        }

        yield return new WaitForSeconds(freezeDuration); // Wait before unfreezing

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None; // Allow movement
        }

        if (col != null)
        {
            col.enabled = true; // Enable interactions
        }
    }

    void DisplayQuantity(GameObject spawnedObj, int quantity)
    {
        GameObject textObj = new GameObject("QuantityText");
        textObj.transform.position = spawnedObj.transform.position + new Vector3(0, 1, 0); // Offset above object
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = quantity.ToString();
        textMesh.fontSize = 24;
        textMesh.color = Color.white;
    }
}
