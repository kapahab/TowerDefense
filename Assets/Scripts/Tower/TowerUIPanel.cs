using UnityEngine;
using TMPro;

public class TowerUIPanel : MonoBehaviour
{
    // THIS is the magic line. It makes the panel globally accessible.
    public static TowerUIPanel Instance { get; private set; }

    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI rateText;

    private void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hide the panel as soon as the game starts (AFTER Awake has run)
        HideTowerInfo();
    }

    public void ShowTowerInfo(string tName, int tLevel, float tDamage, float tRange, float tRate)
    {
        gameObject.SetActive(true);

        nameText.text = tName;
        levelText.text = "Level: " + tLevel.ToString();
        damageText.text = "Damage: " + tDamage.ToString();
        rangeText.text = "Range: " + tRange.ToString();
        rateText.text = "Fire Rate: " + tRate.ToString() + "/s";
    }

    public void HideTowerInfo()
    {
        gameObject.SetActive(false);
    }
}