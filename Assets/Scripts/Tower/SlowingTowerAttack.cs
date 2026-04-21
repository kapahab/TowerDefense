using UnityEngine;

public class SlowingTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    private IDamageable target;
    private ISlowable targetSlowable;

    public float slowAmount = 0.5f; // 50% slow
    public float slowDuration = 2f; // 2 seconds slow
    public void ChooseTarget(Collider[] potentialTargets)
    {
        target = potentialTargets[0].GetComponent<IDamageable>();
        targetSlowable = potentialTargets[0].GetComponent<ISlowable>();
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (target != null)
        {
            target.TakeDamage(data.attackDamage);
        }
        if (targetSlowable != null)
        {
            targetSlowable.Slow(slowAmount, slowDuration);
        }
    }

}
