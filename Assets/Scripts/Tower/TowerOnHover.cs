using UnityEngine;

public class TowerOnHover : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] private ITowerDataContainer dataInst;

    

    // Triggered the exact frame the mouse enters the collider
    private void Start()
    {
        dataInst = GetComponent<ITowerDataContainer>();
    }
    private void OnMouseEnter()
    {
        if (dataInst.GetTowerDataInstance() == null)
        {
            Debug.LogError("TowerDataInstance is null! Make sure the TowerTargetSearch component is properly initialized.");
            return;
        }
        TowerUIPanel.Instance.ShowTowerInfo(dataInst.GetTowerDataInstance().towerName, dataInst.GetTowerDataInstance().currentLevel,
            dataInst.GetTowerDataInstance().attackDamage,
            dataInst.GetTowerDataInstance().attackRange, dataInst.GetTowerDataInstance().attackCooldown);

    }

    // Triggered the exact frame the mouse leaves the collider
    private void OnMouseExit()
    {

        if (dataInst.GetTowerDataInstance() == null)
        {
            Debug.LogError("TowerDataInstance is null! Make sure the TowerTargetSearch component is properly initialized.");
            return;
        }
        TowerUIPanel.Instance.HideTowerInfo();
    }
}