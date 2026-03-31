using System.Collections.Generic;
using UnityEngine;

public interface ITowerAttackStrategy 
{
    public void ChooseTarget(Collider[] potentialTargets);
    public void ExecuteAttack(TowerData data);
}
