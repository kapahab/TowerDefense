using UnityEngine;
using TMPro;

public class ResourceTrackerUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // Subscribe to the UIManager events
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnGoldChanged += UpdateGoldDisplay;
            UIManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        }
    }

    private void OnDestroy()
    {
        // Always unsubscribe to prevent memory leaks
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnGoldChanged -= UpdateGoldDisplay;
            UIManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
        }
    }

    private void UpdateGoldDisplay(int currentGold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {currentGold}";
        }
    }

    private void UpdateScoreDisplay(int currentScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}
