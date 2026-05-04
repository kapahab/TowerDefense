using System;
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



// [Wave and WaveSegment classes remain exactly the same...]

public class LevelManager : MonoBehaviour
{
    [Header("Map Setup")]
    public Transform spawnPoint;
    public Transform baseTarget;
    public Transform[] fullPath;

    [Header("Wave Configuration")]
    public List<Wave> waves;

    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    // NEW: We need to know when the spawner is completely done
    private bool allWavesSpawned = false;

    public static Action OnGameWon;

    void Start()
    {
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        for (int w = 0; w < waves.Count; w++)
        {
            currentWaveIndex = w;
            Wave currentWave = waves[w];

            foreach (WaveSegment segment in currentWave.segments)
            {
                for (int i = 0; i < segment.count; i++)
                {
                    SpawnEnemy(segment.enemyPrefab);
                    yield return new WaitForSeconds(segment.spawnDelay);
                }
            }

            if (w < waves.Count - 1)
            {
                yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
            }
        }

        Debug.Log("All waves have been spawned!");

        // NEW: Tell the manager it's finally allowed to check for a win!
        allWavesSpawned = true;

        // Edge Case: What if the towers killed the very last enemy exactly as it spawned? 
        // We do one final check here just in case.
        if (activeEnemies.Count == 0)
        {
            OnGameWon?.Invoke();
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<EnemyController>().InitializePath(fullPath, baseTarget);

        // 1. Add them to our tracker list! (Your previous code forgot to actually add them)
        activeEnemies.Add(enemy);

        // 2. Subscribe to their death event!
        // (Assuming your enemy health script is called EnemyHealth and has an event that passes itself)
        EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.OnEnemyDied += HandleEnemyDeath;
        }
    }

    // This gets called the moment the enemy's health hits 0
    void HandleEnemyDeath(GameObject deadEnemy)
    {
        // 1. UNSUBSCRIBE IMMEDIATELY! This prevents memory leaks when the enemy is Destroy()'d a millisecond later.
        EnemyHealth healthScript = deadEnemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.OnEnemyDied -= HandleEnemyDeath;
        }

        // 2. Remove them from the active roster
        if (activeEnemies.Contains(deadEnemy))
        {
            activeEnemies.Remove(deadEnemy);
        }

        // 3. Check for the Win Condition safely!
        if (allWavesSpawned && activeEnemies.Count == 0)
        {
            Debug.Log("Level Beaten!");
            OnGameWon?.Invoke();
        }
    }
}