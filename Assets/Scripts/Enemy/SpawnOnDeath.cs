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
        // 1. Grab the parent's target base so we can pass it to the children
        EnemyController parentController = GetComponent<EnemyController>();
        Transform targetBase = parentController != null ? parentController.baseTarget : null;

        for (int i = 0; i < spawnCount; i++)
        {
            // 2. Calculate a small random offset so they don't spawn inside each other
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            // 3. Spawn the child!
            GameObject child = Instantiate(enemyPrefabToSpawn, spawnPos, Quaternion.identity);

            // 4. Give the child its marching orders
            if (targetBase != null)
            {
                EnemyController childController = child.GetComponent<EnemyController>();
                if (childController != null)
                {
                    childController.InitializePath(targetBase);
                }
            }
        }

        Debug.Log($"Enemy died and split into {spawnCount} smaller enemies!");
    }
}