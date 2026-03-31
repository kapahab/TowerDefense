using System.Collections;
using UnityEngine;

public class TowerTargetSearch : MonoBehaviour
{
    private Collider[] enemiesInRange;

    public LayerMask enemyLayer;

    [SerializeField] private TowerData towerData;

    private ITowerAttackStrategy attackStrategy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackStrategy = GetComponent<ITowerAttackStrategy>();
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
            enemiesInRange = Physics.OverlapSphere(transform.position, towerData.attackRange, enemyLayer);
            if (enemiesInRange.Length > 0)
            {
                attackStrategy.ChooseTarget(enemiesInRange);
                attackStrategy.ExecuteAttack(towerData);

                yield return new WaitForSeconds(towerData.attackCooldown);
                continue;
            }
            Debug.Log("No enemies found");
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);
    }
}
