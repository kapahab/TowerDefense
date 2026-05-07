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
    public static LevelManager Instance { get; private set; }

    [Header("Map Setup")]
    public List<RouteConfig> availableRoutes;

    [Header("Wave Configuration")]
    public List<Wave> waves;

    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private bool allWavesSpawned = false;
    private bool skipWaitRequested = false; // NEW: Flag to skip the timer
    private bool isFastForward = false;     // NEW: Tracks our game speed

    // EVENTS
    public static Action OnGameWon;
    public static Action<int, int> OnWaveStarted; // NEW: Passes (CurrentWave, TotalWaves)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Always ensure time is normal when a level starts!
        Time.timeScale = 1f;

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

            // Tell the UI that a new wave has started! (Adding 1 so it reads Wave 1 instead of Wave 0)
            OnWaveStarted?.Invoke(currentWaveIndex + 1, waves.Count);

            foreach (WaveSegment segment in currentWave.segments)
            {
                int safeRouteIndex = segment.routeIndex;
                if (safeRouteIndex < 0 || safeRouteIndex >= availableRoutes.Count) safeRouteIndex = 0;

                RouteConfig selectedRoute = availableRoutes[safeRouteIndex];

                for (int i = 0; i < segment.count; i++)
                {
                    SpawnEnemy(segment.enemyPrefab, selectedRoute);
                    yield return new WaitForSeconds(segment.spawnDelay);
                }
            }

            // NEW: Interruptible Wait Timer between waves!
            if (w < waves.Count - 1)
            {
                float waitTimer = currentWave.timeBeforeNextWave;
                skipWaitRequested = false; // Reset the flag

                // Loop until the timer runs out, OR the player hits the Skip button
                while (waitTimer > 0f && !skipWaitRequested)
                {
                    waitTimer -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
            }
        }

        Debug.Log("All waves have been spawned!");
        allWavesSpawned = true;

        StartCoroutine(CheckWinConditionSafely());
    }

    // --- NEW PUBLIC METHODS FOR UI BUTTONS ---

    public void SkipToNextWave()
    {
        // This instantly breaks the `while` loop in the Coroutine above!
        skipWaitRequested = true;
    }

    public void ToggleFastForward()
    {
        isFastForward = !isFastForward;

        // Time.timeScale controls the speed of EVERYTHING in Unity (physics, animations, coroutines)
        Time.timeScale = isFastForward ? 2f : 1f;

        Debug.Log($"Game Speed set to: {Time.timeScale}x");
    }

    // -----------------------------------------

    void SpawnEnemy(GameObject prefab, RouteConfig route)
    {
        GameObject enemy = Instantiate(prefab, route.spawnPoint.position, route.spawnPoint.rotation);
        enemy.GetComponent<EnemyController>().InitializePath(route.fullPath, route.baseTarget);
        RegisterEnemy(enemy);
    }

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
        if (healthScript != null) healthScript.OnEnemyDied -= HandleEnemyDeath;

        if (activeEnemies.Contains(deadEnemy)) activeEnemies.Remove(deadEnemy);

        StartCoroutine(CheckWinConditionSafely());
    }

    private IEnumerator CheckWinConditionSafely()
    {
        yield return new WaitForEndOfFrame();
        if (allWavesSpawned && activeEnemies.Count == 0)
        {
            Time.timeScale = 1f; // Reset time so the win screen isn't hyper-fast
            OnGameWon?.Invoke();
        }
    }
}