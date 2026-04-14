using UnityEngine;
using UnityEngine.AI;

public class SlowdownTrap : MonoBehaviour, ITrapEffect
{
    [Tooltip("Multiplier for speed (e.g., 0.5 means half speed)")]
    public float speedMultiplier = 0.5f;
    public float slowDuration = 5f;

    public void ApplyEffect(GameObject enemy)
    {
        ISlowable slowTarget = enemy.GetComponent<ISlowable>();

        if (slowTarget != null)
        {
            slowTarget.Slow(speedMultiplier, slowDuration); // Example: slow for 5 seconds
        }
        else
        {
            Debug.LogWarning("Enemy does not implement ISlowable interface.");
        }
    }
}