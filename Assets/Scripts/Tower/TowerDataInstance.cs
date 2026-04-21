[System.Serializable]
public class TowerDataInstance
{
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;

    // NEW: A constructor that copies the stats from the Blueprint into this local copy
    public void InitializeFromBlueprint(TowerData data)
    {
        attackDamage = data.attackDamage;
        attackCooldown = data.attackCooldown;
        attackRange = data.attackRange;
    }
}