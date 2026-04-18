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
            EnemyLoot loot = GetComponent<EnemyLoot>();
            if (loot != null) loot.DropGold();
            Debug.Log("Enemy destroyed!");
            Destroy(this.gameObject);
        }
    }
}