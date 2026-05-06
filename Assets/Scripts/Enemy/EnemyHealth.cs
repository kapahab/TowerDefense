using System;
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 50;
    public float currentHealth;

    private Shield[] activeShields;

    [Header("Game Feel")]
    public Renderer enemyRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    public Action<GameObject> OnEnemyDied;

    void Start()
    {
        currentHealth = maxHealth;
        activeShields = GetComponents<Shield>();

        if (enemyRenderer == null) enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null) originalColor = enemyRenderer.material.color;
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

        // Flash Red for Game Feel!
        if (enemyRenderer != null && !isFlashing)
        {
            StartCoroutine(DamageFlash());
        }

        Debug.Log($"Enemy took {finalDamage} damage to health. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy destroyed!");
            OnEnemyDied?.Invoke(this.gameObject);
            EnemyLoot loot = GetComponent<EnemyLoot>();
            if (loot != null) loot.DropGold();

            //Check if this enemy splits into smaller enemies
            SpawnOnDeath spawner = GetComponent<SpawnOnDeath>();
            if (spawner != null) spawner.SpawnChildren();

            Destroy(this.gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        isFlashing = true;
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (enemyRenderer != null) enemyRenderer.material.color = originalColor;
        isFlashing = false;
    }
}