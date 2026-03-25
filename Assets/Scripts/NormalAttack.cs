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

    public void ChooseTarget(List<Transform> potentialTargets)
    {
        target = potentialTargets[0]; //this is just a placeholder, we will need to implement some sort of target selection logic here, maybe based on distance or health or something else.
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
