using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoonManager : MonoBehaviour
{
    [Header("Boon Database")]
    [Tooltip("Populate this list with all available BoonData ScriptableObjects")]
    [SerializeField] private List<BoonData> masterBoonList;

    [Header("UI References")]
    [SerializeField] private GameObject boonSelectionPanel;
    [SerializeField] private BoonBannerUI[] banners = new BoonBannerUI[3];

    private TowerData currentTargetTower; // To know which tower is being upgraded

    private void Start()
    {
        if (boonSelectionPanel != null) boonSelectionPanel.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnUpgradeTriggered += TriggerRNGSelection;
            UIManager.Instance.OnBoonSelected += ResumeGame;
            
            // Track the currently selected tower for upgrades
            UIManager.Instance.OnTowerSelected += SetTargetTower;
            UIManager.Instance.OnSpaceClicked += ClearTargetTower;
        }
    }

    private void OnDestroy()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnUpgradeTriggered -= TriggerRNGSelection;
            UIManager.Instance.OnBoonSelected -= ResumeGame;
            UIManager.Instance.OnTowerSelected -= SetTargetTower;
            UIManager.Instance.OnSpaceClicked -= ClearTargetTower;
        }
    }

    private void SetTargetTower(TowerData data) { currentTargetTower = data; }
    private void ClearTargetTower() { currentTargetTower = null; }

    /// <summary>
    /// Executes the RNG logic and displays the 3 choices.
    /// </summary>
    public void TriggerRNGSelection()
    {
        if (currentTargetTower == null)
        {
            Debug.LogWarning("BoonManager: No tower is currently selected to upgrade!");
            return;
        }

        if (masterBoonList == null || masterBoonList.Count < 3)
        {
            Debug.LogError("BoonManager: Not enough boons in the master list to offer 3 unique options!");
            return;
        }

        // 1. Weighted Random Selection
        List<BoonData> selectedBoons = GetRandomBoons(3);

        // 2. Populate Banners
        for (int i = 0; i < banners.Length; i++)
        {
            if (i < selectedBoons.Count && banners[i] != null)
            {
                banners[i].gameObject.SetActive(true);
                banners[i].SetupBanner(selectedBoons[i], currentTargetTower);
            }
            else if (banners[i] != null)
            {
                banners[i].gameObject.SetActive(false);
            }
        }

        // 3. Activate UI and Pause Game
        if (boonSelectionPanel != null) boonSelectionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Pulls a unique set of boons based on their assigned weights.
    /// </summary>
    private List<BoonData> GetRandomBoons(int count)
    {
        List<BoonData> pool = new List<BoonData>(masterBoonList);
        List<BoonData> result = new List<BoonData>();

        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0) break;

            int totalWeight = pool.Sum(b => b.weight);
            int randomValue = Random.Range(0, totalWeight);
            int currentWeightSum = 0;

            for (int j = 0; j < pool.Count; j++)
            {
                currentWeightSum += pool[j].weight;
                if (randomValue < currentWeightSum)
                {
                    result.Add(pool[j]);
                    pool.RemoveAt(j); // Ensure unique options by removing the selected one from this temporary pool
                    break;
                }
            }
        }
        return result;
    }

    private void ResumeGame()
    {
        if (boonSelectionPanel != null) boonSelectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
