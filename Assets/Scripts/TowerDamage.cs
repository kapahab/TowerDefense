using System.Collections;
using UnityEngine;

public class TowerDamage : MonoBehaviour
{
    public float attackRange;
    public float attackCooldown;
    public float attackDamage;

    private Collider[] enemiesInRange;

    public LayerMask enemyLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TowerSearchTick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TowerSearchTick()
    {
        while (true)
        {
            enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            if (enemiesInRange.Length > 0)
            {

                IDamageable damageableEnemy = enemiesInRange[0].GetComponent<IDamageable>();
                if (damageableEnemy != null)
                {
                    damageableEnemy.TakeDamage(attackDamage); //example damage value
                    Debug.DrawLine(transform.position, enemiesInRange[0].transform.position, Color.blue, 1f);
                    yield return new WaitForSeconds(attackCooldown);

                    continue;
                }


            }
            Debug.Log("No enemies found");
            yield return new WaitForSeconds(2f);
        }
    }
}
