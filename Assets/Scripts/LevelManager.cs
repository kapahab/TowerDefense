using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. WAVE SEGMENT: A specific group of enemies (e.g., "5 fast enemies spawning 1 second apart")
[System.Serializable]
public class WaveSegment
{
    public GameObject enemyPrefab;
    public int count;
    [Tooltip("Time to wait between spawning each enemy in this group")]
    public float spawnDelay = 1f;
}

// 2. WAVE: The full wave, which can contain multiple segments
[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public List<WaveSegment> segments;

    [Tooltip("Time to wait before the NEXT wave starts")]
    public float timeBeforeNextWave = 10f;
}

// 3. THE MANAGER
public class LevelManager : MonoBehaviour
{
    [Header("Map Setup")]
    public Transform spawnPoint; // Where enemies appear
    public Transform baseTarget; // Where enemies are trying to walk to

    [Header("Wave Configuration")]
    public List<Wave> waves;

    private int currentWaveIndex = 0;

    void Start()
    {
        // Start the level!
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        for (int w = 0; w < waves.Count; w++)
        {
            currentWaveIndex = w;
            Wave currentWave = waves[w];

            Debug.Log($"Starting {currentWave.waveName}!");

            // Go through each segment in the current wave
            foreach (WaveSegment segment in currentWave.segments)
            {
                for (int i = 0; i < segment.count; i++)
                {
                    SpawnEnemy(segment.enemyPrefab);
                    yield return new WaitForSeconds(segment.spawnDelay);
                }
            }

            // The wave has finished spawning. 
            // Wait out the timer before starting the next wave in the list.
            if (w < waves.Count - 1)
            {
                Debug.Log($"Wave finished spawning! Next wave in {currentWave.timeBeforeNextWave} seconds...");
                yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
            }
        }

        Debug.Log("All waves have been spawned! Level complete.");
    }

    void SpawnEnemy(GameObject prefab)
    {
        // Instantiate the enemy at the spawn point
        GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        enemy.GetComponent<EnemyController>().InitializePath(baseTarget);
    }
}