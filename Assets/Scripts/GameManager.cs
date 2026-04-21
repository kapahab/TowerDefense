using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int startingGold = 100;

    public int currentHealth;
    public int maxHealth = 100;

    public static Action OnGameOver;
    public static Action<int> OnPlayerHurt;

    void Start()
    {
        currentHealth = maxHealth;
        EconomyManager.ResetGold(startingGold);
        Debug.Log($"Level started with {EconomyManager.currentGold} gold.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckIncomingDamage(int i)
    {
        currentHealth -= i;

        OnPlayerHurt?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        OnGameOver?.Invoke();
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        DestinationManager.OnEnemyReachedDestination += CheckIncomingDamage;
    }

    private void OnDisable()
    {
        DestinationManager.OnEnemyReachedDestination -= CheckIncomingDamage;
    }
}
