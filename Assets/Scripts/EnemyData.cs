using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float health;
    public float attackDamage;
    public float attackRange;
    public float attackCooldown;
}
