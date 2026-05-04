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
        attackCooldown = data.attackCooldown;
        goldGenerated = data.goldGenerated;
        generationInterval = data.generationInterval;
    }

    // A helper method you can call when the player picks an upgrade
    public void LevelUp(UpgradeChoice choice)
    {
        currentLevel++;
        attackDamage += choice.bonusDamage;
        attackRange += choice.bonusRange;
        attackCooldown = Mathf.Max(0.1f, attackCooldown - choice.bonusFireRate);

        // Apply Economy Upgrades
        goldGenerated += choice.bonusGold;
        generationInterval = Mathf.Max(0.5f, generationInterval - choice.bonusSpeed);
    }
}