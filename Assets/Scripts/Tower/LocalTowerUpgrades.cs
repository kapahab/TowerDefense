using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public enum UpgradeTier
{
    Tier1, // 75%
    Tier2, // 20%
    Tier3  // 5%
}

[System.Serializable]
public class UpgradeChoice
{
    public string upgradeName = "New Upgrade";
    public string description = "+10 Dmg";
    public int cost = 50;

    [Header("Specialization (Tier 0)")]
    public bool isSpecialization = false;
    public GameObject specializedTowerPrefab;

    [Header("Stat Upgrades (Tier 1-3)")]
    public UpgradeTier tier = UpgradeTier.Tier1;
    public bool isPercentageBased = false;

    [Header("Combat Stats")]
    public float bonusDamage;
    public float bonusRange;
    public float bonusFireRate;

    [Header("Special Stats")]
    public float bonusAoERadius;
    public float bonusSlowAmount;
    public float bonusDoTDamage;

    [Header("Economy Stats")]
    public int bonusGold;
    public float bonusSpeed;
}

[RequireComponent(typeof(Collider))]
public class LocalTowerUpgrades : MonoBehaviour
{
    [Header("Specialization (Tier 0)")]
    public bool requiresSpecialization = false;
    public List<UpgradeChoice> specializationPaths;
    private bool isSpecialized = false;

    [Header("Stat Upgrades (Tier 1-3)")]
    public List<UpgradeChoice> possibleUpgrades;
    public int maxUpgrades = 2;
    private int currentUpgrades = 0;

    private ITowerDataContainer dataContainer;
    private List<UpgradeChoice> currentChoices;

    void Awake()
    {
        dataContainer = GetComponent<ITowerDataContainer>();
    }

    void OnMouseDown()
    {
        // 1. Prevent clicking on the tower if the mouse is currently hovering over a UI banner!
        if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        // Tier 0: Check if it needs to specialize first
        if (requiresSpecialization && !isSpecialized)
        {
            if (currentChoices == null || currentChoices.Count == 0)
            {
                currentChoices = specializationPaths.OrderBy(x => Random.value).Take(3).ToList();
            }

            // Prevent opening the UI if they can't even afford the cheapest choice
            if (currentChoices.Count > 0 && EconomyManager.currentGold < currentChoices.Min(c => c.cost))
            {
                Debug.LogWarning("Not enough gold to open Specialization UI!");
                return;
            }

            if (UpgradeUIPanel.Instance != null)
            {
                UpgradeUIPanel.Instance.ShowPanel(this, currentChoices);
            }
            return;
        }

        // Tiers 1-3: Stat Upgrades
        if (currentUpgrades >= maxUpgrades)
        {
            Debug.Log("Tower is already Max Level!");
            return;
        }

        if (currentChoices == null || currentChoices.Count == 0)
        {
            RollWeightedUpgrades();
        }

        // Prevent opening the UI if they can't even afford the cheapest choice
        if (currentChoices.Count > 0 && EconomyManager.currentGold < currentChoices.Min(c => c.cost))
        {
            Debug.LogWarning("Not enough gold to open Upgrade UI!");
            return;
        }

        if (UpgradeUIPanel.Instance != null)
        {
            UpgradeUIPanel.Instance.ShowPanel(this, currentChoices);
        }
    }

    private void RollWeightedUpgrades()
    {
        currentChoices = new List<UpgradeChoice>();
        List<UpgradeChoice> tier1 = possibleUpgrades.Where(u => u.tier == UpgradeTier.Tier1).ToList();
        List<UpgradeChoice> tier2 = possibleUpgrades.Where(u => u.tier == UpgradeTier.Tier2).ToList();
        List<UpgradeChoice> tier3 = possibleUpgrades.Where(u => u.tier == UpgradeTier.Tier3).ToList();

        for (int i = 0; i < 3; i++)
        {
            float roll = Random.Range(0f, 100f);
            List<UpgradeChoice> poolToUse = null;

            if (roll <= 75f && tier1.Count > 0)
            {
                poolToUse = tier1;
            }
            else if (roll <= 95f && tier2.Count > 0)
            {
                poolToUse = tier2;
            }
            else if (tier3.Count > 0)
            {
                poolToUse = tier3;
            }

            // Fallback if the selected pool is empty
            if (poolToUse == null || poolToUse.Count == 0)
            {
                poolToUse = possibleUpgrades; // Just grab from anywhere
            }

            if (poolToUse.Count > 0)
            {
                UpgradeChoice selected = poolToUse[Random.Range(0, poolToUse.Count)];
                currentChoices.Add(selected);
            }
        }
    }

    public void BuyUpgrade(UpgradeChoice chosenUpgrade)
    {
        if (EconomyManager.TrySpendGold(chosenUpgrade.cost))
        {
            if (chosenUpgrade.isSpecialization)
            {
                // Instantiate the specialized tower exactly where this one is
                if (chosenUpgrade.specializedTowerPrefab != null)
                {
                    GameObject newTower = Instantiate(chosenUpgrade.specializedTowerPrefab, transform.position, transform.rotation, transform.parent);
                    
                    // Register the new tower if needed by other systems (like Grid)
                    // The old tower gets destroyed
                    Destroy(gameObject);
                }
            }
            else
            {
                if (dataContainer != null)
                {
                    dataContainer.GetTowerDataInstance().LevelUp(chosenUpgrade);
                    currentUpgrades++;
                    currentChoices.Clear();
                    Debug.Log($"Upgraded {chosenUpgrade.upgradeName}!");
                }
                else
                {
                    Debug.LogError("No ITowerDataContainer found on this tower! Make sure the tower has a script that implements it.");
                }
            }

            if (UpgradeUIPanel.Instance != null)
            {
                UpgradeUIPanel.Instance.HidePanel();
            }
        }
        else
        {
            Debug.LogWarning("Not enough gold!");
        }
    }
}