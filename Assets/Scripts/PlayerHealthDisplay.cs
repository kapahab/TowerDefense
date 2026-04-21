using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxHealth = 100;
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = $"Health: {maxHealth}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth(int i)
    {
        text.text = $"Health: {i}";
    }

    void OnEnable()
    {
        GameManager.OnPlayerHurt += UpdateHealth;
    }

    void OnDisable()
    {
        GameManager.OnPlayerHurt -= UpdateHealth;
    }
}
