using UnityEngine;
using UnityEngine.UI;

public class ActionBarUI : MonoBehaviour
{
    [Header("Action Buttons")]
    [SerializeField] private Button towersButton;
    [SerializeField] private Button trapsButton;
    [SerializeField] private Button upgradesButton;

    private void Awake()
    {
        if (towersButton != null) towersButton.onClick.AddListener(OnTowersClicked);
        if (trapsButton != null) trapsButton.onClick.AddListener(OnTrapsClicked);
        if (upgradesButton != null) upgradesButton.onClick.AddListener(OnUpgradesClicked);
    }

    private void OnTowersClicked()
    {
        Debug.Log("Build Mode: Towers Selected");
        // State change logic for building towers goes here
    }

    private void OnTrapsClicked()
    {
        Debug.Log("Build Mode: Traps Selected");
        // State change logic for building traps goes here
    }

    private void OnUpgradesClicked()
    {
        Debug.Log("Action: Upgrades Triggered");
        if (UIManager.Instance != null)
        {
            UIManager.Instance.TriggerUpgrade();
        }
    }
}
