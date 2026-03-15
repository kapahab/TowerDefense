using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    [SerializeField]private EnemyAttack attack;
    [SerializeField] private Movement movement;
    public EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        StartCoroutine(AITick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNewTarget(Transform newTarget)
    {
        target = newTarget;
        attack.attackTarget = newTarget;
        movement.MoveTo(newTarget);
        currentState = EnemyState.Moving;
    }


    private IEnumerator AITick() //might need to switch to state pattern for more states.
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Moving:
                    if (target != null && Vector3.Distance(target.position, transform.position) <= attack.attackRange)
                    {
                        movement.StopMoving();
                        currentState = EnemyState.Attacking;
                    }
                    break;
                case EnemyState.Attacking:
                    if (target == null)
                    {
                        currentState = EnemyState.Idle;
                    }
                    else
                    {
                        attack.SingleAttack();

                        yield return new WaitForSeconds(attack.attackCooldown);
                        continue;
                    }

                    break;

            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    
    private void OnEnable()
    {
        EnemyManager.OnNextTower += GetNewTarget;
    }

    private void OnDisable()
    {
        EnemyManager.OnNextTower -= GetNewTarget;
    }


}
