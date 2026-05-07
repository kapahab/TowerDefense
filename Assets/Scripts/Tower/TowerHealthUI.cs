using UnityEngine;
using UnityEngine.UI; // Required for the Image component

public class TowerHealthUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TowerHealth towerHealth;

    [Tooltip("The Image component set to 'Filled'")]
    [SerializeField] private Image healthBarFill;

    void Start()
    {
        // Automatically grab the health script from the parent if we forgot to drag it in
        if (towerHealth == null)
        {
            towerHealth = GetComponentInParent<TowerHealth>();
        }
    }

    void Update()
    {
        // Update the visual fill amount every frame
        if (towerHealth != null && healthBarFill != null)
        {
            // We cast to (float) to prevent integer division from snapping the bar directly to 0
            healthBarFill.fillAmount = (float)towerHealth.currentHealth / (float)towerHealth.maxHealth;
        }
    }
}