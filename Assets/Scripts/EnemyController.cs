using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking
}

public class EnemyController : MonoBehaviour //tells a single enemy what to do
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    
    [SerializeField] private EnemyData data; //this is reused in both attackstrategy and here, there might be a better way to do this
    public EnemyState currentState = EnemyState.Idle;

    private EnemyHealth health;
    private Movement movement;
    private IAttackStrategy attack; //maybe make the enemies as types so that they can have different attacks? or just make the attack script more flexible with different attack types and values.

    void Awake()
    {
        movement = GetComponent<Movement>();

        health = GetComponent<EnemyHealth>();
        health.maxHealth = data.health;

        attack = GetComponent<IAttackStrategy>();
    }

    void Start()
    {
        
        StartCoroutine(AITick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNewTarget(List<Transform> newTargets) //this should now get the list for all available targets
    {
        target = attack.ChooseTarget(newTargets);
        movement.MoveTo(target);
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
                    if (target != null && Vector3.Distance(target.position, transform.position) <= data.attackRange)
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
                        attack.ExecuteAttack(data);

                        yield return new WaitForSeconds(data.attackCooldown);
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
