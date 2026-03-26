using UnityEngine;
using System.Collections.Generic;
public interface IAttackStrategy 
{

    public Transform ChooseTarget(List<Transform> potentialTargets);
    public void ExecuteAttack(EnemyData data);

    
}
