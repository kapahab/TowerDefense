using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 50;
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
            Debug.Log("Enemy destroyed!");
            Destroy(this.gameObject); //?
        }

        maxHealth -= damage;
    }
}
