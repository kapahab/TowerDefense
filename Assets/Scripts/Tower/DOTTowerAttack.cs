using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DoTTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("DoT Stats")]
    [Tooltip("The total amount of damage dealt over the entire duration")]
    public float totalDotDamage = 15f;
    [Tooltip("How long the poison/burn lasts in seconds")]
    public float dotDuration = 3f;
    [Tooltip("How many times the enemy takes damage during the duration")]
    public int damageTicks = 3;

    [Header("Visuals")]
    public GameObject projectilePrefab;
    [Tooltip("The poison bubbles or flames that stick to the enemy")]
    public GameObject dotStatusPrefab;
    public Transform firePoint;

    [Header("Bubble Animation Settings")]
    [Tooltip("How high the entire group of bubbles floats up over the DoT duration")]
    public float bubbleFloatHeight = 1.5f;
    [Tooltip("How far each individual bubble bobs up and down")]
    public float childBobAmount = 0.2f;
    [Tooltip("How fast each individual bubble completes one up/down cycle")]
    public float childBobSpeed = 0.4f;

    [Header("Audio")]
    public AudioClip attackSound;

    private IDamageable targetDamageable;
    private Transform targetTransform;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        targetTransform = potentialTargets[0].transform;
        targetDamageable = potentialTargets[0].GetComponent<IDamageable>();
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (targetTransform != null)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject visualFX = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                VisualProjectile visualScript = visualFX.GetComponent<VisualProjectile>();

                if (visualScript != null)
                {
                    visualScript.PlayVisual(targetTransform);

                    float distance = Vector3.Distance(firePoint.position, targetTransform.position);
                    float flightTime = distance / visualScript.speed;

                    PlayAttackSound();
                    StartCoroutine(ApplyDoTAfterDelay(flightTime, data.attackDamage));
                }
            }
            else
            {
                PlayAttackSound();
                StartCoroutine(ApplyDoTAfterDelay(0f, data.attackDamage));
            }
        }
        else
        {
            Debug.Log("target transform null");
        }
    }

    private IEnumerator ApplyDoTAfterDelay(float delayTime, float initialImpactDamage)
    {
        yield return new WaitForSeconds(delayTime);

        if (targetTransform != null && targetDamageable != null)
        {
            if (initialImpactDamage > 0)
            {
                targetDamageable.TakeDamage(initialImpactDamage);
            }

            if (dotStatusPrefab != null)
            {
                GameObject statusAura = Instantiate(dotStatusPrefab, targetTransform.position, Quaternion.identity, targetTransform);
                statusAura.transform.localPosition = new Vector3(0, 0.1f, 0);

                // 1. Move the parent group upwards over the entire duration safely
                statusAura.transform.DOLocalMoveY(bubbleFloatHeight, dotDuration).SetEase(Ease.Linear).SetLink(statusAura);

                // 2. Loop through every child bubble inside the group
                foreach (Transform childBubble in statusAura.transform)
                {
                    // Give each bubble a random start delay (0.0 to 0.5 seconds) so they bob out of sync
                    float randomDelay = Random.Range(0f, 0.5f);

                    // Record where the bubble currently is on the Y axis
                    float startY = childBubble.localPosition.y;

                    // DOTween Magic: Move it up slightly, ping-pong it back down, and repeat infinitely!
                    // .SetLink(childBubble.gameObject) ensures it doesn't throw errors when the enemy dies
                    childBubble.DOLocalMoveY(startY + childBobAmount, childBobSpeed)
                               .SetDelay(randomDelay)
                               .SetEase(Ease.InOutSine)
                               .SetLoops(-1, LoopType.Yoyo)
                               .SetLink(childBubble.gameObject);
                }

                Destroy(statusAura, dotDuration);
            }

            float damagePerTick = totalDotDamage / damageTicks;
            float timeBetweenTicks = dotDuration / damageTicks;

            for (int i = 0; i < damageTicks; i++)
            {
                if (targetTransform == null || targetDamageable == null) yield break;

                targetDamageable.TakeDamage(damagePerTick);
                yield return new WaitForSeconds(timeBetweenTicks);
            }
        }
        else
        {
            if (targetTransform == null)
                Debug.Log("target transform null");
            if (targetDamageable == null)
                Debug.Log("target damageable null");
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