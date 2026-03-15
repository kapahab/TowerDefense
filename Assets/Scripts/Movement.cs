using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Transform targetTransform)
    {
        agent.isStopped = false;
        agent.SetDestination(targetTransform.position);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }
}
