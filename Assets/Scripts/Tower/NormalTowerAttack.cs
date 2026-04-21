using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NormalTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    private IDamageable target;
    public void ChooseTarget(Collider[] potentialTargets)
    {
        target = potentialTargets[0].GetComponent<IDamageable>();
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (target != null)
            target.TakeDamage(data.attackDamage);
    }
}
