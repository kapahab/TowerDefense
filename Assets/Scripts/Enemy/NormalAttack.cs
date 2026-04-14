using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour, IAttackStrategy
{
    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform ChooseTarget(List<Transform> potentialTargets)
    {
        return target = potentialTargets[0]; //for selecting the closest tower(The comparison is done in the enemy manager)
    }
    public void ExecuteAttack(EnemyData data)
    {
        IDamageable damageableTarget = target.GetComponent<IDamageable>();
        if (damageableTarget != null)
        {
            damageableTarget.TakeDamage(data.attackDamage);
            Debug.DrawLine(transform.position, target.position, Color.red, 2f);
        }
    }
}
