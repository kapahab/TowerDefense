using System;
using UnityEngine;

public class TowerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public static Action OnTowerDestroyed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (maxHealth <= 0)
        {
            // Handle tower destruction logic here (e.g., play animation, disable tower, etc.)
            Debug.Log("Tower destroyed!");
            OnTowerDestroyed?.Invoke();
            Destroy(this.gameObject); //?
        }

        maxHealth -= damage;
    }
}
