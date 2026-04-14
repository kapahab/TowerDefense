using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour, ISlowable
{
    [SerializeField] NavMeshAgent agent;
    bool isSlowed = false;
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

    public void Slow(float slowAmount, float slowDuration)
    {
        if (isSlowed) return; // Prevent stacking slows
        StartCoroutine(SlowCoroutine(slowAmount, slowDuration));
    }

    IEnumerator SlowCoroutine(float slowAmount, float slowDuration)
    {
        isSlowed = true;
        float originalSpeed = agent.speed;
        agent.speed *= (1 - slowAmount);
        yield return new WaitForSeconds(slowDuration);
        agent.speed = originalSpeed;
        isSlowed = false;
    }
}
