using System.Collections;
using UnityEngine;

public class DOTTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    public int damageTickAmount;
    public float damageTickDuration;

    private IDamageable target;
    public void ChooseTarget(Collider[] potentialTargets)
    {
        target = potentialTargets[0].GetComponent<IDamageable>();
    }

    public void ExecuteAttack(TowerData data)
    {
        StartCoroutine(DamageTick(data));
    }

    IEnumerator DamageTick(TowerData data)
    {
        for (int i = 0; i < damageTickAmount; i++)
        {
            if (target as MonoBehaviour != null)
                target.TakeDamage(data.attackDamage);
            yield return new WaitForSeconds(damageTickDuration);
        }
    }

}
