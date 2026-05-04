using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // Phase 2: Static HUD Events
    public event Action<int> OnGoldChanged;
    public event Action<int> OnScoreChanged;

    // Phase 3: Contextual UI Events
    public event Action<TowerData> OnTowerSelected;
    public event Action OnSpaceClicked;

    // Phase 4: RNG Upgrade Events
    public event Action OnUpgradeTriggered;
    public event Action OnBoonSelected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // --- Event Invokers ---

    public void UpdateGold(int currentGold)
    {
        OnGoldChanged?.Invoke(currentGold);
    }

    public void UpdateScore(int currentScore)
    {
        OnScoreChanged?.Invoke(currentScore);
    }

    public void SelectTower(TowerData towerData)
    {
        OnTowerSelected?.Invoke(towerData);
    }

    public void DeselectTower()
    {
        OnSpaceClicked?.Invoke();
    }

    public void TriggerUpgrade()
    {
        OnUpgradeTriggered?.Invoke();
    }

    public void CloseBoonSelection()
    {
        OnBoonSelected?.Invoke();
    }
}
