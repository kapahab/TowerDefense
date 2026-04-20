using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 50;
    public float currentHealth;

    private Shield[] activeShields;

    void Start()
    {
        currentHealth = maxHealth;

        activeShields = GetComponents<Shield>();
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage;

        if (activeShields != null && activeShields.Length > 0)
        {
            foreach (Shield shield in activeShields)
            {
                finalDamage = shield.ProcessDamage(finalDamage);

                if (finalDamage <= 0)
                {
                    return;
                }
            }
        }

        currentHealth -= finalDamage;
        Debug.Log($"Enemy took {finalDamage} damage to health. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy destroyed!");

            EnemyLoot loot = GetComponent<EnemyLoot>();
            if (loot != null) loot.DropGold();

            //Check if this enemy splits into smaller enemies
            SpawnOnDeath spawner = GetComponent<SpawnOnDeath>();
            if (spawner != null) spawner.SpawnChildren();

            Destroy(this.gameObject);
        }
    }
}