using UnityEngine;

public class TrapPlacement : MonoBehaviour
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public int trapCost = 50;

    [Header("The 'Fake' Grid Settings")]
    public float gridSize = 1f;
    public float gridOffset = 0.5f;

    [Header("Layer Masks")]
    public LayerMask groundLayer;
    public LayerMask restrictedLayers;

    [Header("Ghost Visuals")]
    public GameObject ghostTrapPrefab;
    [Tooltip("Color when the spot is empty and safe to place")]
    public Color validColor = new Color(0f, 1f, 0f, 0.5f);   // Semi-transparent Green
    [Tooltip("Color when hovering over a rock, wall, or another trap")]
    public Color invalidColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent Red

    private GameObject currentGhost;
    private Renderer[] ghostRenderers; // Grabs all renderers in case your trap has multiple parts

    private bool isPlacingMode = false;

    public void ActivateTrapPlacement()
    {
        isPlacingMode = true;

        if (ghostTrapPrefab != null && currentGhost == null)
        {
            currentGhost = Instantiate(ghostTrapPrefab);
            // Cache the renderers once so we don't kill performance searching for them every frame
            ghostRenderers = currentGhost.GetComponentsInChildren<Renderer>();
        }

        Debug.Log("Placement mode activated. Waiting for click...");
    }

    void Update()
    {
        if (!isPlacingMode) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);

            // Check if the spot is valid THIS frame
            bool isSpotValid = !Physics.CheckSphere(snappedPos, gridSize * 0.4f, restrictedLayers);

            if (currentGhost != null)
            {
                currentGhost.SetActive(true);
                currentGhost.transform.position = snappedPos;

                // Change the color based on if the spot is blocked!
                SetGhostColor(isSpotValid ? validColor : invalidColor);
            }

            // Listen for the Click
            if (Input.GetMouseButtonDown(0))
            {
                AttemptPlacement(snappedPos, isSpotValid);
            }
        }
        else
        {
            if (currentGhost != null) currentGhost.SetActive(false);
        }

        // Cancel placement on Right-Click
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    // We pass the boolean here so we don't have to run the physics sphere check twice
    void AttemptPlacement(Vector3 placementPos, bool isSpotValid)
    {
        if (!isSpotValid)
        {
            Debug.LogWarning("Placement failed: Something is already here!");
            // TIP: Add a little "Bzzzt" error sound effect here!
            return;
        }

        if (EconomyManager.TrySpendGold(trapCost))
        {
            Instantiate(trapPrefab, placementPos, Quaternion.identity);
            Debug.Log("Trap successfully placed on the grid!");
            CancelPlacement();
        }
        else
        {
            Debug.LogWarning("Not enough gold to place this trap!");
            CancelPlacement();
        }
    }

    private void SetGhostColor(Color targetColor)
    {
        if (ghostRenderers == null) return;

        // Loop through the base and all child objects to tint the whole trap
        foreach (Renderer r in ghostRenderers)
        {
            r.material.color = targetColor;
        }
    }

    private void CancelPlacement()
    {
        isPlacingMode = false;
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }
    }

    private Vector3 SnapToGrid(Vector3 rawPosition)
    {
        float x = Mathf.Round((rawPosition.x - gridOffset) / gridSize) * gridSize + gridOffset;
        float z = Mathf.Round((rawPosition.z - gridOffset) / gridSize) * gridSize + gridOffset;
        return new Vector3(x, rawPosition.y, z);
    }
}