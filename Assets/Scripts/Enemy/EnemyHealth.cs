using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 50;
    private float currentHealth;

    // Array to hold any charge-based shields attached to this GameObject
    private Shield[] activeShields;

    void Start()
    {
        currentHealth = maxHealth;

        // Automatically find any scripts on this enemy that inherit from the Shield base class
        activeShields = GetComponents<Shield>();
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage;

        // 1. Pass the incoming damage through all active shields first
        if (activeShields != null && activeShields.Length > 0)
        {
            foreach (Shield shield in activeShields)
            {
                // The health script doesn't need to know the rules of the shield.
                // It just passes the damage and waits for the return value.
                finalDamage = shield.ProcessDamage(finalDamage);

                // If the shield returns 0, it means the attack was absorbed or deflected.
                // The attack is "spent", so we stop processing entirely.
                if (finalDamage <= 0)
                {
                    return;
                }
            }
        }

        // 2. Apply whatever damage successfully made it past all shields
        currentHealth -= finalDamage;
        Debug.Log($"Enemy took {finalDamage} damage to health. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy destroyed!");
            Destroy(this.gameObject);
        }
    }
}