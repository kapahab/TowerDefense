using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("Visuals")]
    public GameObject arrowVisualPrefab;

    // NEW: Add a slot for your explosion!
    public GameObject explosionPrefab;

    public Transform firePoint;

    private IDamageable targetDamageable;
    private Transform targetTransform;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        targetTransform = potentialTargets[0].transform;
        targetDamageable = potentialTargets[0].GetComponent<IDamageable>();
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (targetTransform != null && targetDamageable != null)
        {
            GameObject visualArrow = Instantiate(arrowVisualPrefab, firePoint.position, firePoint.rotation);
            VisualProjectile visualScript = visualArrow.GetComponent<VisualProjectile>();
            visualScript.PlayVisual(targetTransform);

            float distance = Vector3.Distance(firePoint.position, targetTransform.position);
            float timeToHit = distance / visualScript.speed;

            StartCoroutine(DealDamageAfterDelay(data.attackDamage, timeToHit));
        }
    }

    private IEnumerator DealDamageAfterDelay(float damage, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (targetTransform != null)
        {
            // NEW: Spawn the explosion directly on the enemy's chest!
            if (explosionPrefab != null)
            {
                // We add Vector3.up * 0.5f so the explosion happens on their body, not at their feet
                Vector3 hitPoint = targetTransform.position + (Vector3.up * 0.5f);

                // Spawn it, and tell Unity to delete the explosion object after 2 seconds so it doesn't clutter your game
                GameObject boom = Instantiate(explosionPrefab, hitPoint, Quaternion.identity);
                Destroy(boom, 2f);
            }

            targetDamageable.TakeDamage(damage);
        }
    }
}