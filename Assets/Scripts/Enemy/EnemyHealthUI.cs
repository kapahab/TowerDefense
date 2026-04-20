using System;
using TMPro;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] TextMeshPro healthText;
    private Shield shield;
    void Start()
    {
        shield = GetComponentInParent<Shield>();
    }

    private void Update()
    {
        if (shield != null)
        {
            healthText.text = "Health: " + health.currentHealth + "/n Shield Charge: " + shield.shieldCharges;

        }
        else
        {
            healthText.text = "Health: " + health.currentHealth;
        }
    }
}
