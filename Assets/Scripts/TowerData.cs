using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]

public class TowerData : ScriptableObject
{
    public string towerName;
    public float health;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
}
