using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("Core Scripts")]
    [SerializeField] private EnemyHealth health;
    private Shield shield;

    [Header("Health Visuals")]
    [Tooltip("The Image component set to 'Filled' for the health bar")]
    [SerializeField] private Image healthBarFill;

    [Header("Shield Charge Visuals")]
    [Tooltip("The parent background object holding the shield sprites")]
    [SerializeField] private GameObject shieldContainer;
    [Tooltip("Put the individual filled shield sprites in this array")]
    [SerializeField] private Image[] shieldChargeSprites;

    [Header("Damage Burst Shield Visuals")]
    [Tooltip("The Image component set to 'Filled' that shows progress to breaking the next shield charge")]
    [SerializeField] private Image burstShieldFill;

    void Start()
    {
        shield = GetComponentInParent<Shield>();

        // Hide the standard shield pips container if there is no shield at all
        if (shield == null && shieldContainer != null)
        {
            shieldContainer.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateHealthBar();

        if (shield != null)
        {
            UpdateShieldCharges();
            UpdateBurstShieldBar();
        }
        else if (burstShieldFill != null)
        {
            // If they have no shield, ensure the burst bar is hidden
            burstShieldFill.gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null && health != null)
        {
            healthBarFill.fillAmount = (float)health.currentHealth / (float)health.maxHealth;
        }
    }

    private void UpdateShieldCharges()
    {
        if (shieldChargeSprites == null) return;

        for (int i = 0; i < shieldChargeSprites.Length; i++)
        {
            shieldChargeSprites[i].enabled = (i < shield.shieldCharges);
        }
    }

    private void UpdateBurstShieldBar()
    {
        if (burstShieldFill == null) return;

        // NEW: Check if the enemy's shield is specifically the FlatArmorShield!
        if (shield is FlatArmorShield flatShield)
        {
            // Make sure the bar is visible
            if (!burstShieldFill.gameObject.activeSelf)
                burstShieldFill.gameObject.SetActive(true);

            // Get the current burst damage and divide it by the required damage to get a 0-1 percentage
            float progress = flatShield.GetCurrentDamageProgress();
            burstShieldFill.fillAmount = progress / flatShield.requiredDamage;
        }
        else
        {
            // If they have a regular shield (not a FlatArmorShield), hide this specific fill bar
            burstShieldFill.gameObject.SetActive(false);
        }
    }
}