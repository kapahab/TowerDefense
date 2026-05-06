using UnityEngine;
using System;

public class EnemyLoot : MonoBehaviour
{
    public int goldValue = 15;

    // Static event that passes the death location and the base gold value
    public static event Action<Vector3, int> OnEnemyDied;

    public void DropGold()
    {
        EconomyManager.AddGold(goldValue);
        OnEnemyDied?.Invoke(transform.position, goldValue);
        Debug.Log($"Enemy died and dropped {goldValue} gold! Total Gold: {EconomyManager.currentGold}");
    }
}
