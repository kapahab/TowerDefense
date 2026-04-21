using System;
using UnityEngine;
using UnityEngine.AI;

public class DestinationManager : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    public static Action<int> OnEnemyReachedDestination;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("enemy entered trigger");
            TeleportToStartingPoint(other.gameObject);
            OnEnemyReachedDestination?.Invoke(1); //every enemy damages one
        }
    }

    void TeleportToStartingPoint(GameObject enemy)
    {
        // 1. Grab the agent and Warp it (this safely teleports it without breaking the NavMesh)
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(startPoint.position);
        }
        else
        {
            // Fallback just in case the enemy doesn't have an agent
            enemy.transform.position = startPoint.position;
        }

        // Even though they teleported, their brain still thinks they are at the end.
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            // tell them to start walking toward it again from the new spawn position.
            controller.InitializePath(this.transform);
        }
    }
}
