using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]

public class TowerData : ScriptableObject
{
    public string towerName;
    public float health;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;

    [Header("Random Upgrade Pool")]
    [Tooltip("Fill this with 5-10 different upgrades. The game will pick 3 random ones.")]
    public List<UpgradeChoice> upgradePool;
}
