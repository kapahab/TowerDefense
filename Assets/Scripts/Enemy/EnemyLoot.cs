using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public int goldValue = 15;

    public void DropGold()
    {
        EconomyManager.AddGold(goldValue);
        Debug.Log($"Enemy died and dropped {goldValue} gold! Total Gold: {EconomyManager.currentGold}");
    }
}
