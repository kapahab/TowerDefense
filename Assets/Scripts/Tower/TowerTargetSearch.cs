using System.Collections;
using UnityEngine;

public class TowerTargetSearch : MonoBehaviour
{
    private Collider[] enemiesInRange;

    public LayerMask enemyLayer;

    [SerializeField] public TowerData towerData;
    public TowerDataInstance towerDataInst = new TowerDataInstance();

    private ITowerAttackStrategy attackStrategy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackStrategy = GetComponent<ITowerAttackStrategy>();

        if (towerData != null)
        {
            towerDataInst.InitializeFromBlueprint(towerData);
        }

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
            enemiesInRange = Physics.OverlapSphere(transform.position, towerDataInst.attackRange, enemyLayer);

            if (enemiesInRange.Length > 0)
            {
                // 1. Enemy found! Execute the strategy.
                attackStrategy.ChooseTarget(enemiesInRange);
                attackStrategy.ExecuteAttack(towerDataInst);

                // 2. Go on cooldown. The tower cannot shoot or search again until this is over.
                yield return new WaitForSeconds(towerDataInst.attackCooldown);
            }
            else
            {
                // 3. No enemies found. Wait just a tiny fraction of a second before checking again.
                // This keeps performance high but makes the tower react almost instantly.
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerDataInst.attackRange);
    }
}
