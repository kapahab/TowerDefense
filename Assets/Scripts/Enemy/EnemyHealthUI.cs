using System;
using TMPro;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] TextMeshPro healthText;


    private void Update()
    {
        healthText.text = "Health: " + health.currentHealth;
    }
}
