using UnityEngine;

public class ScavengerTower : MonoBehaviour, ITowerDataContainer
{
    [Header("Tower Data")]
    [SerializeField] public TowerData towerData;
    public TowerDataInstance towerDataInst = new TowerDataInstance();

    [Header("Effects")]
    public ParticleSystem coinEffect;
    public AudioClip coinSound;

    void Start()
    {
        if (towerData != null)
        {
            towerDataInst.InitializeFromBlueprint(towerData);
        }
    }

    public TowerDataInstance GetTowerDataInstance()
    {
        return towerDataInst;
    }

    private void OnEnable()
    {
        EnemyLoot.OnEnemyDied += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyLoot.OnEnemyDied -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Vector3 deathPos, int baseGoldValue)
    {
        // Check if the enemy died within our scavenge radius (using attackRange)
        if (Vector3.Distance(transform.position, deathPos) <= towerDataInst.attackRange)
        {
            EconomyManager.AddGold(towerDataInst.goldGenerated);

            if (coinEffect != null)
            {
                coinEffect.Play();
            }

            if (coinSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(coinSound, 1f, true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Need to check if it's initialized for the editor view
        float radius = towerDataInst != null && towerDataInst.attackRange > 0 ? towerDataInst.attackRange : 10f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
