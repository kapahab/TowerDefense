using TMPro;
using UnityEngine;

public class DisplayGold : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI goldText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGoldDisplay(int newGoldAmount)
    {
        goldText.text = "Gold: " + newGoldAmount.ToString();
    }

    void OnEnable()
    {
        EconomyManager.OnGoldChanged += UpdateGoldDisplay;
        UpdateGoldDisplay(EconomyManager.currentGold);
    }

    void OnDisable()
    {
        EconomyManager.OnGoldChanged -= UpdateGoldDisplay;
    }
}
