using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPropertiesPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject propertiesPanel;
    [SerializeField] private Image towerIcon;
    [SerializeField] private TextMeshProUGUI tierText;
    [SerializeField] private TextMeshProUGUI towerNameText;
    
    [Header("Stats References")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI fireRateText;

    private void Start()
    {
        if (propertiesPanel != null) propertiesPanel.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnTowerSelected += PopulateAndShowPanel;
            UIManager.Instance.OnSpaceClicked += HidePanel;
        }
    }

    private void OnDestroy()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnTowerSelected -= PopulateAndShowPanel;
            UIManager.Instance.OnSpaceClicked -= HidePanel;
        }
    }

    private void PopulateAndShowPanel(TowerData towerData)
    {
        if (towerData == null || propertiesPanel == null) return;

        if (towerIcon != null) 
        {
            towerIcon.sprite = towerData.towerIcon;
            towerIcon.enabled = (towerData.towerIcon != null); // Hide image component if no sprite assigned
        }

        if (tierText != null) tierText.text = $"Tier {towerData.towerTier}";
        if (towerNameText != null) towerNameText.text = towerData.towerName;
        
        if (damageText != null) damageText.text = $"Damage: {towerData.attackDamage}";
        if (rangeText != null) rangeText.text = $"Range: {towerData.attackRange}";
        if (fireRateText != null) fireRateText.text = $"Fire Rate: {towerData.attackCooldown}s";

        propertiesPanel.SetActive(true);
    }

    private void HidePanel()
    {
        if (propertiesPanel != null) propertiesPanel.SetActive(false);
    }
}
