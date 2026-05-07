using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIFuncButtons : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI waveCounterText;

    public Image oneXSpeedIcon;
    public Image twoXSpeedIcon;

    private void OnEnable()
    {
        LevelManager.OnWaveStarted += UpdateWaveText;
    }

    private void OnDisable()
    {
        LevelManager.OnWaveStarted -= UpdateWaveText;
    }

    private void Start()
    {
        // Let the code automatically fix the icons the moment the scene loads!
        // (Make sure BOTH are turned ON in the Unity Editor for this to work perfectly)
        SyncSpeedIcons();
    }

    private void UpdateWaveText(int currentWave, int totalWaves)
    {
        if (waveCounterText != null)
        {
            waveCounterText.text = $"x{totalWaves - currentWave}";
        }
    }

    // Button 1: Send Next Wave
    public void OnCallWaveButtonClicked()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SkipToNextWave();
        }
    }

    // Button 2: Speed Up Time
    public void OnSpeedUpButtonClicked()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ToggleFastForward();
            SyncSpeedIcons(); // Update the UI immediately after toggling
        }
    }

    // A helper method so we don't write the same code twice
    private void SyncSpeedIcons()
    {
        if (oneXSpeedIcon != null && twoXSpeedIcon != null)
        {
            bool isFastForward = Time.timeScale > 1f;

            // Note: I swapped this logic so the 2x icon shows WHEN it is fast-forwarding.
            // If you want the icon to show what the button WILL do, just swap these back!
            twoXSpeedIcon.enabled = isFastForward;
            oneXSpeedIcon.enabled = !isFastForward;
        }
    }
}