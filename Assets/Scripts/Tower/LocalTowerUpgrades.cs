using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq; // Required for shuffling the list

// 1. DEFINE THE UPGRADE DATA STRUCTURE
[System.Serializable]
public class UpgradeChoice
{
    public string upgradeName = "New Upgrade";
    public string description = "+10 Dmg";
    public int cost = 50;

    [Header("Stat Modifiers")]
    public float bonusDamage;
    public float bonusRange;
    public float bonusFireRate;
}

// 2. THE MAIN SCRIPT
[RequireComponent(typeof(Collider))] // Ensures you have a collider to click on
public class LocalTowerUpgrades : MonoBehaviour
{
    [Header("Define Your Upgrades Here!")]
    [Tooltip("Add all your possible upgrades for this specific tower type here.")]
    public List<UpgradeChoice> possibleUpgrades;

    [Header("Upgrade Limits")]
    public int maxUpgrades = 2;
    private int currentUpgrades = 0;

    [Header("Local UI References")]
    public GameObject localCanvas; // The Canvas sitting on this prefab
    public Button[] choiceButtons = new Button[3];
    public TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[3];

    private TowerTargetSearch towerLogic;
    private List<UpgradeChoice> currentChoices; // Remembers the 3 rolled choices
    private bool isMenuOpen = false;

    void Awake()
    {
        towerLogic = GetComponent<TowerTargetSearch>();
        localCanvas.SetActive(false); // Make sure the menu is hidden when the tower spawns
    }

    // 3. DETECT CLICKS ON THE TOWER
    void OnMouseDown()
    {
        isMenuOpen = !isMenuOpen; // Toggle the menu state
        localCanvas.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            RefreshLocalUI();
        }
    }

    // 4. ROLL AND DISPLAY CHOICES
    void RefreshLocalUI()
    {
        if (currentUpgrades >= maxUpgrades)
        {
            // Tower is max level, hide the buttons or show "MAX LEVEL"
            foreach (var btn in choiceButtons) btn.gameObject.SetActive(false);
            return;
        }

        // Roll 3 random choices if we haven't already
        if (currentChoices == null || currentChoices.Count == 0)
        {
            // Shuffle the list and grab the first 3
            currentChoices = possibleUpgrades.OrderBy(x => Random.value).Take(3).ToList();
        }

        // Apply the choices to our 3 buttons
        for (int i = 0; i < 3; i++)
        {
            if (i < currentChoices.Count)
            {
                UpgradeChoice choice = currentChoices[i];
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = $"{choice.upgradeName}\n{choice.cost}g\n{choice.description}";

                // Wire up the button click event
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => BuyUpgrade(choice));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false); // Hide button if we ran out of choices
            }
        }
    }

    // 5. BUY AND APPLY THE UPGRADE
    public void BuyUpgrade(UpgradeChoice chosenUpgrade)
    {
        if (EconomyManager.TrySpendGold(chosenUpgrade.cost))
        {
            // Apply stats directly to your tower data
            towerLogic.towerData.attackDamage += chosenUpgrade.bonusDamage;
            towerLogic.towerData.attackRange += chosenUpgrade.bonusRange;

            // Note: Make sure your TowerData actually has an attacksPerSecond or similar variable!
            towerLogic.towerData.attackCooldown -= chosenUpgrade.bonusFireRate;

            currentUpgrades++;
            currentChoices.Clear(); // Wipe the choices so it rolls new ones next time

            Debug.Log($"Upgraded {chosenUpgrade.upgradeName}!");

            // Close the menu after buying
            isMenuOpen = false;
            localCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Not enough gold!");
        }
    }
}