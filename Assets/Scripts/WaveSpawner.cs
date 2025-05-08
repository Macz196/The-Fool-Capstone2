using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject prefab;        // The prefab to spawn
    public Transform player;         // Reference to the player's transform
    public float spawnRadius = 10f;  // The radius around the player where prefabs can spawn
    public float spawnInterval = 2f; // Time interval between spawning new objects
    public int waveSize = 5;         // Number of objects in each wave
    public float speed = 2f;         // Speed of the prefab moving toward the player

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    // Spawns a wave of objects in intervals
    private IEnumerator SpawnWave()
    {
        while (true)
        {
            for (int i = 0; i < waveSize; i++)
            {
                SpawnPrefab();
                yield return new WaitForSeconds(spawnInterval); // Delay between each spawn in the wave
            }

            yield return new WaitForSeconds(3f); // Delay between waves
        }
    }

    // Spawns the prefab at a random position within a radius
    private void SpawnPrefab()
    {
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;
        GameObject spawnedPrefab = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Make the prefab move towards the player
        StartCoroutine(MoveTowardsPlayer(spawnedPrefab));
    }

    // Coroutine that moves the prefab towards the player
    private IEnumerator MoveTowardsPlayer(GameObject obj)
    {
        while (obj != null)
        {
            Vector2 direction = (player.position - obj.transform.position).normalized;
            obj.transform.position = Vector2.MoveTowards(obj.transform.position, player.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
