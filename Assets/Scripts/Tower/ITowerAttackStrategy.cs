using System.Collections.Generic;
using UnityEngine;

public interface ITowerAttackStrategy 
{
    public void ChooseTarget(Collider[] potentialTargets);
    public void ExecuteAttack(TowerDataInstance data);
    public void PlayAttackSound();
}
