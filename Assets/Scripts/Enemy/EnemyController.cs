//using System;
//using System.Collections;
//using UnityEngine;
//using System.Collections.Generic;

//public enum EnemyState
//{
//    Idle,
//    Moving,
//    Attacking
//}

//public class EnemyController : MonoBehaviour //tells a single enemy what to do
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    public Transform target;

//    [SerializeField] private EnemyData data; //this is reused in both attackstrategy and here, there might be a better way to do this
//    public EnemyState currentState = EnemyState.Idle;

//    private EnemyHealth health;
//    private Movement movement;
//    private IAttackStrategy attack; //maybe make the enemies as types so that they can have different attacks? or just make the attack script more flexible with different attack types and values.

//    void Awake()
//    {
//        movement = GetComponent<Movement>();

//        health = GetComponent<EnemyHealth>();
//        health.maxHealth = data.health;

//        attack = GetComponent<IAttackStrategy>();
//    }

//    void Start()
//    {

//        StartCoroutine(AITick());
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void GetNewTarget(List<Transform> newTargets) //this should now get the list for all available targets
//    {
//        target = attack.ChooseTarget(newTargets);
//        movement.MoveTo(target);
//        currentState = EnemyState.Moving;
//    }


//    private IEnumerator AITick() //might need to switch to state pattern for more states.
//    {
//        while (true)
//        {
//            switch (currentState)
//            {
//                case EnemyState.Idle:
//                    break;
//                case EnemyState.Moving:
//                    if (target != null && Vector3.Distance(target.position, transform.position) <= data.attackRange)
//                    {
//                        movement.StopMoving();
//                        currentState = EnemyState.Attacking;
//                    }
//                    break;
//                case EnemyState.Attacking:
//                    if (target == null)
//                    {
//                        currentState = EnemyState.Idle;
//                    }
//                    else
//                    {
//                        attack.ExecuteAttack(data);

//                        yield return new WaitForSeconds(data.attackCooldown);
//                        continue;
//                    }

//                    break;

//            }

//            yield return new WaitForSeconds(0.2f);
//        }
//    }



//    private void OnEnable()
//    {
//        EnemyManager.OnNextTower += GetNewTarget;
//    }

//    private void OnDisable()
//    {
//        EnemyManager.OnNextTower -= GetNewTarget;
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, data.attackRange);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform baseTarget;   // The ultimate goal at the end of the map

    [SerializeField] private EnemyData data;
    public LayerMask towerLayer;

    private EnemyHealth health;
    private Movement movement;
    private IAttackStrategy attack;

    public Transform[] currentPath;

    void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<EnemyHealth>();

        if (data != null)
        {
            health.maxHealth = data.health;
        }

        attack = GetComponent<IAttackStrategy>();
    }

    // Call this from your LevelManager right after you Instantiate the enemy
    public void InitializePath(Transform[] fullPath, Transform endGoal, int startingWaypointIndex = 0)
    {
        currentPath = fullPath;
        baseTarget = endGoal;

        if (movement != null && fullPath != null)
        {
            // Pass it to the movement script
            movement.SetPath(fullPath, startingWaypointIndex);
        }

        StartCoroutine(AITick());
    }

    // Add this helper so the SpawnOnDeath script can ask the controller for the index
    public int GetWaypointIndex()
    {
        return movement != null ? movement.GetCurrentWaypointIndex() : 0;
    }

    private IEnumerator AITick()
    {
        // Safety check to ensure InitializePath was called before running the loop
        if (baseTarget == null) yield break;

        while (true)
        {
            // 1. Scan for towers within hitting distance
            Collider[] towersInRange = Physics.OverlapSphere(transform.position, data.attackRange, towerLayer);

            if (towersInRange.Length > 0)
            {
                List<Transform> towerTransforms = new List<Transform>();
                foreach (Collider col in towersInRange)
                {
                    towerTransforms.Add(col.transform);
                }

                // 2. Let the strategy pick the specific tower
                Transform attackTarget = attack.ChooseTarget(towerTransforms);

                if (attackTarget != null)
                {
                    // 3. FIRE ON THE MOVE!
                    attack.ExecuteAttack(data);

                    // Wait for the full cooldown before they can shoot again
                    yield return new WaitForSeconds(data.attackCooldown);
                    continue; // Skip the 0.1f wait below and start the loop over
                }
            }

            // Fast tick rate for snappy detection while walking
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        if (data != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.attackRange);
        }
    }
}
