using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerDataInstance
{
    // A reference back to the original ScriptableObject for things that NEVER change
    // (like the Name, Icon, Tier, and the Upgrade Pool)
    public TowerData baseData;

    [Header("Mutable Stats")]
    public string towerName;
    public int currentLevel;
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public int goldGenerated;
    public float generationInterval;

    [Header("Mutable Special Stats")]
    public float bonusAoERadius;
    public float bonusSlowAmount;
    public float bonusDoTDamage;

    // The Constructor: This runs the exact moment the tower is spawned
    public void InitializeFromBlueprint(TowerData data)
    {
        baseData = data;

        currentLevel = 1;

        // Copy the base stats over so they can be modified safely
        maxHealth = data.health;
        currentHealth = data.health;
        attackDamage = data.attackDamage;
        attackRange = data.attackRange;
        
        // Lower attack speed to 66% of base (meaning it takes longer to fire)
        attackCooldown = data.attackCooldown / 0.66f; 
        
        goldGenerated = data.goldGenerated;
        generationInterval = data.generationInterval;

        bonusAoERadius = 0f;
        bonusSlowAmount = 0f;
        bonusDoTDamage = 0f;
    }

    // A helper method you can call when the player picks an upgrade
    public void LevelUp(UpgradeChoice choice)
    {
        currentLevel++;

        if (choice.isPercentageBased)
        {
            attackDamage += attackDamage * (choice.bonusDamage / 100f);
            attackRange += attackRange * (choice.bonusRange / 100f);
            attackCooldown -= attackCooldown * (choice.bonusFireRate / 100f);

            // Note: If base special stat is entirely handled in the attack script,
            // multiplying a 0 bonus by a % will result in 0. To fix this, designers
            // should either use flat bonuses for the first upgrade, or we add the base to data instance.
            bonusAoERadius += bonusAoERadius * (choice.bonusAoERadius / 100f);
            bonusSlowAmount += bonusSlowAmount * (choice.bonusSlowAmount / 100f);
            bonusDoTDamage += bonusDoTDamage * (choice.bonusDoTDamage / 100f);
            
            goldGenerated += Mathf.RoundToInt(goldGenerated * (choice.bonusGold / 100f));
            generationInterval -= generationInterval * (choice.bonusSpeed / 100f);
        }
        else
        {
            attackDamage += choice.bonusDamage;
            attackRange += choice.bonusRange;
            attackCooldown -= choice.bonusFireRate;

            bonusAoERadius += choice.bonusAoERadius;
            bonusSlowAmount += choice.bonusSlowAmount;
            bonusDoTDamage += choice.bonusDoTDamage;

            goldGenerated += choice.bonusGold;
            generationInterval -= choice.bonusSpeed;
        }

        attackCooldown = Mathf.Max(0.1f, attackCooldown);
        generationInterval = Mathf.Max(0.5f, generationInterval);
    }
}