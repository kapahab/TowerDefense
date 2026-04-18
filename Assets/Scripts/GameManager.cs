using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int startingGold = 100;

    void Start()
    {
        EconomyManager.ResetGold(startingGold);
        Debug.Log($"Level started with {EconomyManager.currentGold} gold.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
