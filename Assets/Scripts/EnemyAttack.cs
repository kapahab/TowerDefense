using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public Transform attackTarget;
    void Start()
    {
        //StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (attackTarget != null && Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
            {
                IDamageable damageableTarget = attackTarget.GetComponent<IDamageable>();
                if (damageableTarget != null)
                {
                    damageableTarget.TakeDamage(attackDamage);
                }

                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                break;
            }
        }
    }

    public void SingleAttack()
    {
        IDamageable damageableTarget = attackTarget.GetComponent<IDamageable>();
        if (damageableTarget != null)
        {
            damageableTarget.TakeDamage(attackDamage);
            Debug.DrawLine(transform.position,attackTarget.position,Color.red,2f);
        }
    }
}
