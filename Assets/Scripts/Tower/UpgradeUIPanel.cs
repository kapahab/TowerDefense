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

        for (int i = 0; i < 3; i++)
        {
            if (i < choices.Count)
            {
                UpgradeChoice choice = choices[i];
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = $"{choice.upgradeName}\n{choice.cost}g\n{choice.description}";

                // Clear old listeners so we don't buy an upgrade from a previous tower!
                choiceButtons[i].onClick.RemoveAllListeners();

                // Tell the button to trigger our local method, passing the specific choice
                choiceButtons[i].onClick.AddListener(() => OnUpgradeButtonClicked(choice));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false); // Hide unused panels
            }
        }
    }

    private void OnUpgradeButtonClicked(UpgradeChoice choice)
    {
        // Tell the specific tower that opened the menu to buy this exact upgrade
        if (activeTower != null)
        {
            activeTower.BuyUpgrade(choice);
        }
    }

    public void HidePanel()
    {
        panelContainer.SetActive(false);
        activeTower = null; // Clear the reference
    }
}