using UnityEngine;
using UnityEngine.AI;

public class TrapPlacement : MonoBehaviour
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public float trapRadius = 1.5f;

    [Header("Layer Masks")]
    public LayerMask groundLayer;
    public LayerMask trapLayer;

    [Header("NavMesh Settings")]
    public float navMeshTolerance = 1.0f;

    // NEW: Tracks whether we are currently allowed to place a trap
    private bool isPlacingMode = false;

    // NEW: Call this method from your UI Button's OnClick event
    public void ActivateTrapPlacement()
    {
        isPlacingMode = true;
        Debug.Log("Placement mode activated. Waiting for click...");
    }

    void Update()
    {
        // NEW: Only check for mouse clicks if placement mode is active
        if (isPlacingMode && Input.GetMouseButtonDown(0))
        {
            AttemptPlacement();
        }
    }

    void AttemptPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, navMeshTolerance, NavMesh.AllAreas))
            {
                Collider[] overlappingTraps = Physics.OverlapSphere(navHit.position, trapRadius, trapLayer);

                if (overlappingTraps.Length == 0)
                {
                    Instantiate(trapPrefab, navHit.position, Quaternion.identity);
                    Debug.Log("Trap successfully placed!");

                    // NEW: Turn off placement mode after a successful spawn
                    isPlacingMode = false;
                }
                else
                {
                    Debug.LogWarning("Placement failed: Another trap is already in this spot.");
                }
            }
            else
            {
                Debug.LogWarning("Placement failed: Location is not on the NavMesh.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = Camera.main != null ? Camera.main.ScreenPointToRay(Input.mousePosition) : new Ray();
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            Gizmos.DrawWireSphere(hit.point, trapRadius);
        }
    }
}