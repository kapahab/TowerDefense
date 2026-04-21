using UnityEngine;

public class AreaTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    private Collider[] targetsToHit;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        // For AoE, our "chosen" targets are simply all of them
        targetsToHit = potentialTargets;
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (targetsToHit == null || targetsToHit.Length == 0) return;

        foreach (Collider enemy in targetsToHit)
        {
            IDamageable health = enemy.GetComponent<IDamageable>();
            if (health != null)
            {
                health.TakeDamage(data.attackDamage);
            }
        }

        Debug.Log($"AoE Tower zapped {targetsToHit.Length} enemies!");
    }
}