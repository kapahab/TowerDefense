using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq; // Required for shuffling the list

[System.Serializable]
public class UpgradeChoice
{
    public string upgradeName = "New Upgrade";
    public string description = "+10 Dmg";
    public int cost = 50;

    [Header("Combat Stats")]
    public float bonusDamage;
    public float bonusRange;
    public float bonusFireRate;

    [Header("Economy Stats")]
    public int bonusGold;      // How much extra gold it makes
    public float bonusSpeed;   // How much faster it prints money
}

// 2. THE MAIN SCRIPT
[RequireComponent(typeof(Collider))] // Ensures you have a collider to click on
public class LocalTowerUpgrades : MonoBehaviour
{
    [Header("Upgrade Data")]
    public List<UpgradeChoice> possibleUpgrades;
    public int maxUpgrades = 2;
    private int currentUpgrades = 0;

    private TowerTargetSearch towerLogic;
    private List<UpgradeChoice> currentChoices;

    void Awake()
    {
        towerLogic = GetComponent<TowerTargetSearch>();
    }

    void OnMouseDown()
    {
        // If max level, maybe play an error sound and do nothing
        if (currentUpgrades >= maxUpgrades)
        {
            Debug.Log("Tower is already Max Level!");
            return;
        }

        // Roll 3 random choices if we haven't already
        if (currentChoices == null || currentChoices.Count == 0)
        {
            currentChoices = possibleUpgrades.OrderBy(x => Random.value).Take(3).ToList();
        }

        // Call the Singleton UI and pass THIS specific script as the target
        if (UpgradeUIPanel.Instance != null)
        {
            UpgradeUIPanel.Instance.ShowPanel(this, currentChoices);
        }
    }

    public void BuyUpgrade(UpgradeChoice chosenUpgrade)
    {
        if (EconomyManager.TrySpendGold(chosenUpgrade.cost))
        {
            // IMPORTANT: Apply stats to the LIVE DATA instance, not the base ScriptableObject!
            // Assuming you added the LevelUp method we discussed previously.
            towerLogic.towerDataInst.LevelUp(chosenUpgrade);

            currentUpgrades++;
            currentChoices.Clear(); // Wipe the choices so it rolls new ones next time

            Debug.Log($"Upgraded {chosenUpgrade.upgradeName}!");

            // Close the global UI after a successful purchase
            if (UpgradeUIPanel.Instance != null)
            {
                UpgradeUIPanel.Instance.HidePanel();
            }
        }
        else
        {
            Debug.LogWarning("Not enough gold!");
            // You could also trigger a red flash on the UI here!
        }
    }
}