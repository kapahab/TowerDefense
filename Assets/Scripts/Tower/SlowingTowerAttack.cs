using System.Collections;
using UnityEngine;

public class SlowingTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("Tower Stats")]
    public float slowAmount = 0.5f; // 50% slow
    public float slowDuration = 2f; // 2 seconds slow

    [Header("Visuals")]
    public GameObject projectilePrefab;   // Drag your dummy flying object here
    public Transform firePoint;           // Empty GameObject at the tip of the tower
    [Tooltip("The ice block or aura that sticks to the enemy while slowed")]
    public GameObject slowStatusPrefab;

    [Header("Audio")]
    public AudioClip attackSound;

    private IDamageable targetDamageable;
    private ISlowable targetSlowable;
    private Transform targetTransform;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        // Cache all the components we need, plus the physical transform for the visuals
        targetTransform = potentialTargets[0].transform;
        targetDamageable = potentialTargets[0].GetComponent<IDamageable>();
        targetSlowable = potentialTargets[0].GetComponent<ISlowable>();
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (targetTransform != null)
        {
            // 1. Spawn the flying projectile dummy
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject visualFX = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                VisualProjectile visualScript = visualFX.GetComponent<VisualProjectile>();

                if (visualScript != null)
                {
                    visualScript.PlayVisual(targetTransform);

                    // Calculate flight time
                    float distance = Vector3.Distance(firePoint.position, targetTransform.position);
                    float flightTime = distance / visualScript.speed;

                    PlayAttackSound();
                    // Start the timer to delay the actual damage and slow
                    StartCoroutine(ApplyEffectAfterDelay(data, flightTime));
                }
            }
            else
            {
                PlayAttackSound();
                // Failsafe: If no visuals are assigned in the editor, just apply the effect instantly
                StartCoroutine(ApplyEffectAfterDelay(data, 0f));
            }
        }
    }

    private IEnumerator ApplyEffectAfterDelay(TowerDataInstance data, float delayTime)
    {
        // Wait exactly as long as it takes the dummy projectile to fly
        yield return new WaitForSeconds(delayTime);

        // Make sure the enemy didn't die while the snowball was mid-air
        if (targetTransform != null)
        {
            // 1. Apply Damage
            if (targetDamageable != null)
            {
                targetDamageable.TakeDamage(data.attackDamage);
            }

            // 2. Apply Slow and Status Visual
            if (targetSlowable != null)
            {
                float totalSlow = slowAmount + data.bonusSlowAmount;
                targetSlowable.Slow(totalSlow, slowDuration);

                // 3. Spawn the Ice Aura!
                if (slowStatusPrefab != null)
                {
                    // Instantiate the aura AT the enemy's position, and make the enemy its Parent!
                    // This means the aura will automatically walk around with the enemy.
                    GameObject statusAura = Instantiate(slowStatusPrefab, targetTransform.position, Quaternion.identity, targetTransform);

                    // We lift it up slightly so it doesn't clip into the floor
                    statusAura.transform.localPosition = new Vector3(0, 0.1f, 0);

                    // Tell Unity to automatically delete the aura the exact millisecond the slow wears off
                    Destroy(statusAura, slowDuration);
                }
            }
        }
    }

    public void PlayAttackSound()
    {
        if (attackSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(attackSound, 1f, true);
        }
    }
}