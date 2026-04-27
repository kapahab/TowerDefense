using UnityEngine;
using UnityEngine.AI;

public class SpawnOnDeath : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefabToSpawn;
    public int spawnCount = 2;

    [Tooltip("How far apart they spread out when they spawn")]
    public float spawnRadius = 1.0f;

    // We call this right before the parent is destroyed
    public void SpawnChildren()
    {
        EnemyController parentController = GetComponent<EnemyController>();
        Transform targetBase = parentController != null ? parentController.baseTarget : null;
        Transform[] parentPath = parentController != null ? parentController.currentPath : null;

        // NEW: Grab the exact waypoint index the parent was walking towards
        int parentIndex = parentController != null ? parentController.GetWaypointIndex() : 0;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            GameObject child = Instantiate(enemyPrefabToSpawn, spawnPos, Quaternion.identity);

            if (targetBase != null && parentPath != null)
            {
                EnemyController childController = child.GetComponent<EnemyController>();
                if (childController != null)
                {
                    // NEW: Pass the parent's index so the baby resumes the exact same route!
                    childController.InitializePath(parentPath, targetBase, parentIndex);
                }
            }
        }

        Debug.Log($"Enemy died and split into {spawnCount} smaller enemies!");
    }
}