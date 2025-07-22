using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // Drag your prefab here in Inspector
    public int asteroidCount = 5;     // Number to spawn at game start

    void Start()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        // Spawn asteroids within camera view but not in the very center
        float spawnRadius = 7f; // adjust to fit your camera size

        Vector2 spawnPos;
        do
        {
            spawnPos = Random.insideUnitCircle * spawnRadius;
        } while (spawnPos.magnitude < 2.5f); // avoid very center

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
    }
}
