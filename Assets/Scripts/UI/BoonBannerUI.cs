using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoonBannerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Button bannerButton;

    private BoonData currentBoon;
    private TowerData targetTower;

    public void SetupBanner(BoonData boon, TowerData towerToUpgrade)
    {
        currentBoon = boon;
        targetTower = towerToUpgrade;

        if (iconImage != null) 
        {
            iconImage.sprite = boon.icon;
            iconImage.enabled = (boon.icon != null);
        }
        
        if (nameText != null) nameText.text = boon.boonName;
        if (descriptionText != null) descriptionText.text = boon.description;
        
        string stats = "";
        if (boon.bonusDamage != 0) stats += $"Damage +{boon.bonusDamage}\n";
        if (boon.bonusRange != 0) stats += $"Range +{boon.bonusRange}\n";
        if (boon.bonusFireRate != 0) stats += $"Fire Rate +{boon.bonusFireRate}\n";
        
        if (statsText != null) statsText.text = stats;

        if (bannerButton != null)
        {
            bannerButton.onClick.RemoveAllListeners();
            bannerButton.onClick.AddListener(ApplyBoon);
        }
    }

    private void ApplyBoon()
    {
        if (targetTower != null && currentBoon != null)
        {
            // Apply modifiers directly to the TowerData instance
            targetTower.attackDamage += currentBoon.bonusDamage;
            targetTower.attackRange += currentBoon.bonusRange;
            targetTower.attackCooldown -= currentBoon.bonusFireRate; // Reducing cooldown increases fire rate
            
            // Notify UI Manager to close the panel and resume the game
            if (UIManager.Instance != null)
            {
                UIManager.Instance.CloseBoonSelection();
            }
        }
    }
}
