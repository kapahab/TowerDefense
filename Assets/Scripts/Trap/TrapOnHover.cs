using System;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class TrapOnHover : MonoBehaviour
{
    public string trapName;
    public string trapDescription;
    private void OnMouseEnter()
    {
        TowerUIPanel.Instance.ShowTowerInfo(trapName, 1,
        0,
        0, 0);
    }
    private void OnMouseExit()
    {
        TowerUIPanel.Instance.HideTowerInfo();
    }
}
