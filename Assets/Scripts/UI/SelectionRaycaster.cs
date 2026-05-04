using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask selectableLayer; // Assign the Tower layer here in the Inspector

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Block raycast if clicking on a UI element (like the pause menu or action bar)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) 
                return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, selectableLayer))
            {
                // Attempt to retrieve TowerTargetSearch to get the TowerData blueprint
                TowerTargetSearch tower = hit.collider.GetComponentInParent<TowerTargetSearch>();
                
                if (tower != null && tower.towerData != null)
                {
                    UIManager.Instance.SelectTower(tower.towerData);
                }
                else
                {
                    UIManager.Instance.DeselectTower();
                }
            }
            else
            {
                UIManager.Instance.DeselectTower();
            }
        }
    }
}
