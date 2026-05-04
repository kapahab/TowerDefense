using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteConfig
{
    public string routeName = "Route 1";
    public Transform spawnPoint;
    public Transform[] fullPath;
    public Transform baseTarget;
}

[System.Serializable]
public class WaveSegment
{
    public GameObject enemyPrefab;
    public int count;
    [Tooltip("Time to wait between spawning each enemy in this group")]
    public float spawnDelay = 1f;

    [Header("Routing")]
    public int routeIndex = 0;
}

[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public List<WaveSegment> segments;
    public float timeBeforeNextWave = 10f;
}

public class LevelManager : MonoBehaviour
{
    // NEW: Singleton Instance so spawned babies can talk to the manager!
    public static LevelManager Instance { get; private set; }

    [Header("Map Setup")]
    public List<RouteConfig> availableRoutes;

    [Header("Wave Configuration")]
    public List<Wave> waves;

    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private bool allWavesSpawned = false;

    public static Action OnGameWon;

    void Awake()
    {
        // Singleton Setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (availableRoutes == null || availableRoutes.Count == 0)
        {
            Debug.LogError("LevelManager has no routes assigned!");
            return;
        }

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
                int safeRouteIndex = segment.routeIndex;
                if (safeRouteIndex < 0 || safeRouteIndex >= availableRoutes.Count)
                {
                    safeRouteIndex = 0;
                }

                RouteConfig selectedRoute = availableRoutes[safeRouteIndex];

                for (int i = 0; i < segment.count; i++)
                {
                    SpawnEnemy(segment.enemyPrefab, selectedRoute);
                    yield return new WaitForSeconds(segment.spawnDelay);
                }
            }

            if (w < waves.Count - 1)
            {
                yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
            }
        }

        Debug.Log("All waves have been spawned!");
        allWavesSpawned = true;

        // Trigger the safe check instead of doing it instantly
        StartCoroutine(CheckWinConditionSafely());
    }

    void SpawnEnemy(GameObject prefab, RouteConfig route)
    {
        GameObject enemy = Instantiate(prefab, route.spawnPoint.position, route.spawnPoint.rotation);
        enemy.GetComponent<EnemyController>().InitializePath(route.fullPath, route.baseTarget);

        // Use our new public registration method!
        RegisterEnemy(enemy);
    }

    // NEW: A public method that ANY script (like SpawnOnDeath) can call to add an enemy to the tracker
    public void RegisterEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);

        EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.OnEnemyDied += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath(GameObject deadEnemy)
    {
        EnemyHealth healthScript = deadEnemy.GetComponent<EnemyHealth>();
        if (healthScript != null)
        {
            healthScript.OnEnemyDied -= HandleEnemyDeath;
        }

        if (activeEnemies.Contains(deadEnemy))
        {
            activeEnemies.Remove(deadEnemy);
        }

        // Use the safe checker!
        StartCoroutine(CheckWinConditionSafely());
    }

    // NEW: This waits until the absolute end of the Unity frame before checking for a win.
    // This guarantees that if a parent dies and spawns babies, the babies have time to Register before we count to 0!
    private IEnumerator CheckWinConditionSafely()
    {
        yield return new WaitForEndOfFrame();

        if (allWavesSpawned && activeEnemies.Count == 0)
        {
            Debug.Log("Level Beaten!");
            OnGameWon?.Invoke();
        }
    }
}