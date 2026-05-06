using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUIPanel : MonoBehaviour
{
    // The Singleton instance
    public static UpgradeUIPanel Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject panelContainer; // The parent object holding the 3 panels
    public Button[] choiceButtons = new Button[3];
    public TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[3];

    // CRITICAL: We remember WHICH tower opened this menu!
    private LocalTowerUpgrades activeTower;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        HidePanel();
    }

    // The tower calls this and passes itself + its rolled choices
    public void ShowPanel(LocalTowerUpgrades callingTower, List<UpgradeChoice> choices)
    {
        activeTower = callingTower;
        panelContainer.SetActive(true);

        // Enter Slow-Mo!
        Time.timeScale = 0.15f; 
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Keep physics smooth

        for (int i = 0; i < 3; i++)
        {
            if (i < choices.Count)
            {
                UpgradeChoice choice = choices[i];
                choiceButtons[i].gameObject.SetActive(true);
                
                string finalDesc = GenerateStatDescription(choice);
                choiceTexts[i].text = $"{choice.upgradeName}\n{choice.cost}g\n{finalDesc}";

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnUpgradeButtonClicked(choice));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private string GenerateStatDescription(UpgradeChoice choice)
    {
        if (choice.isSpecialization) return choice.description;

        string desc = "";
        string pct = choice.isPercentageBased ? "%" : "";

        if (choice.bonusDamage > 0) desc += $"+{choice.bonusDamage}{pct} Damage\n";
        if (choice.bonusRange > 0) desc += $"+{choice.bonusRange}{pct} Range\n";
        if (choice.bonusFireRate > 0) desc += $"+{choice.bonusFireRate}{pct} Fire Rate\n";
        
        if (choice.bonusAoERadius > 0) desc += $"+{choice.bonusAoERadius}{pct} AoE Radius\n";
        if (choice.bonusSlowAmount > 0) desc += $"+{choice.bonusSlowAmount}{pct} Slow\n";
        if (choice.bonusDoTDamage > 0) desc += $"+{choice.bonusDoTDamage}{pct} Poison Dmg\n";

        if (choice.bonusGold > 0) desc += $"+{choice.bonusGold}{pct} Gold Income\n";
        if (choice.bonusSpeed > 0) desc += $"+{choice.bonusSpeed}{pct} Proc Speed\n";

        if (string.IsNullOrEmpty(desc)) return choice.description; // Fallback
        
        return desc.TrimEnd('\n');
    }

    private void OnUpgradeButtonClicked(UpgradeChoice choice)
    {
        if (activeTower != null)
        {
            activeTower.BuyUpgrade(choice);
        }
    }

    public void HideIfActive(LocalTowerUpgrades tower)
    {
        if (activeTower == tower)
        {
            HidePanel();
        }
    }

    public void HidePanel()
    {
        panelContainer.SetActive(false);
        activeTower = null;

        // Restore Normal Time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}