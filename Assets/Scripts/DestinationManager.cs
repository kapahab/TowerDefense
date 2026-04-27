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
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.Warp(startPoint.position);
        else enemy.transform.position = startPoint.position;

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            // NEW: Pass the enemy's own saved path and target back to it!
            controller.InitializePath(controller.currentPath, controller.baseTarget);
        }
    }
}
