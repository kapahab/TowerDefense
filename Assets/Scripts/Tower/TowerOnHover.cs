using UnityEngine;

public class TowerOnHover : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] private ITowerDataContainer dataInst;

    public int segments = 50; 
    private LineRenderer line;

    // Triggered the exact frame the mouse enters the collider
    private void Start()
    {
        dataInst = GetComponent<ITowerDataContainer>();
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        HideRing();
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
        DrawRing(dataInst.GetTowerDataInstance().attackRange);
        ShowRing();

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
        HideRing();
    }

    public void DrawRing(float radius)
    {
        float angle = 0f;
        for (int i = 0; i < (segments + 1); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            // Notice we lift Y by 0.05f here too, to prevent ground clipping!
            line.SetPosition(i, new Vector3(x, 0.05f, z));

            angle += (360f / segments);
        }
    }
    public void ShowRing()
    {
        line.enabled = true;
    }

    public void HideRing()
    {
        line.enabled = false;
    }
}