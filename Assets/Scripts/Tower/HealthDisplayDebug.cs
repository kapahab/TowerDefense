using TMPro;
using UnityEngine;

public class HealthDisplayDebug : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TowerHealth towerHealth;
    private TextMeshPro text;
    void Start()
    {
        towerHealth = GetComponentInParent<TowerHealth>();
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Health: " + towerHealth.maxHealth;
    }
}
