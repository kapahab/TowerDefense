using UnityEngine;

public class TowerOnHover : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] private TowerTargetSearch towerTargetSearch;

    

    // Triggered the exact frame the mouse enters the collider
    private void Start()
    {
        towerTargetSearch = GetComponent<TowerTargetSearch>();
    }
    private void OnMouseEnter()
    {
        if (towerTargetSearch.towerDataInst == null)
        {
            Debug.LogError("TowerDataInstance is null! Make sure the TowerTargetSearch component is properly initialized.");
            return;
        }
        TowerUIPanel.Instance.ShowTowerInfo(towerTargetSearch.towerDataInst.towerName, towerTargetSearch.towerDataInst.currentLevel,
            towerTargetSearch.towerDataInst.attackDamage,
            towerTargetSearch.towerDataInst.attackRange, towerTargetSearch.towerDataInst.attackCooldown);

    }

    // Triggered the exact frame the mouse leaves the collider
    private void OnMouseExit()
    {

        if (towerTargetSearch.towerDataInst == null)
        {
            Debug.LogError("TowerDataInstance is null! Make sure the TowerTargetSearch component is properly initialized.");
            return;
        }
        TowerUIPanel.Instance.HideTowerInfo();
    }
}