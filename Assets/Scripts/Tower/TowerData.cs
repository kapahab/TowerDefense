using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]

public class TowerData : ScriptableObject
{
    public string towerName;
    public Sprite towerIcon;
    [Range(1, 3)] public int towerTier = 1;
    public float health;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
    public int goldGenerated;
    public float generationInterval;


}
