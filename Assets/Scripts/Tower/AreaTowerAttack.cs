using System.Collections;
using UnityEngine;

public class AreaTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("AoE Stats")]
    public float baseAoeRadius = 3f;
    public LayerMask enemyLayer; // Needed to find targets on explosion

    [Header("Visuals")]
    [Tooltip("The boulder or bomb that flies through the air")]
    public GameObject arcProjectilePrefab;
    [Tooltip("The explosion or crater left on the ground")]
    public GameObject explosionCraterPrefab;
    public Transform firePoint;

    [Header("Audio")]
    public AudioClip attackSound;

    private Vector3 impactPoint;
    private LayerMask cachedEnemyLayer;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        // Aim exactly at the feet of the first enemy in the cluster
        if (potentialTargets.Length > 0 && potentialTargets[0] != null)
        {
            impactPoint = potentialTargets[0].transform.position;
            // Lock the Y axis to the floor so the math is perfect
            impactPoint.y = 0f;
            
            // Auto-detect the enemy layer in case it wasn't set in the Inspector
            cachedEnemyLayer = 1 << potentialTargets[0].gameObject.layer;
        }
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        float flightTime = 0f;

        // 1. Launch the Mortar Visual
        if (arcProjectilePrefab != null && firePoint != null)
        {
            GameObject visualFX = Instantiate(arcProjectilePrefab, firePoint.position, firePoint.rotation);
            ArcVisualsProjectile visualScript = visualFX.GetComponent<ArcVisualsProjectile>();

            if (visualScript != null)
            {
                visualScript.PlayVisual(impactPoint);
                flightTime = Vector3.Distance(firePoint.position, impactPoint) / visualScript.speed;
            }
        }

        PlayAttackSound();

        // 2. Start the delayed explosion
        StartCoroutine(ApplyAoEAfterDelay(flightTime, data));
    }

    private IEnumerator ApplyAoEAfterDelay(float delayTime, TowerDataInstance data)
    {
        // Wait while the boulder is mid-air
        yield return new WaitForSeconds(delayTime);

        // 1. Spawn the Explosion/Crater visual
        if (explosionCraterPrefab != null)
        {
            // Lift it 0.05f so it sits perfectly on top of the floor without clipping (z-fighting)
            Vector3 spawnPos = new Vector3(impactPoint.x, 0.05f, impactPoint.z);

            // Spawn it facing straight up from the ground
            GameObject boom = Instantiate(explosionCraterPrefab, spawnPos, Quaternion.Euler(-90, 0, 0));

            // Delete the crater after 3 seconds so the map doesn't get cluttered
            Destroy(boom, 3f);
        }

        // 2. Deal Damage to everyone in the true AoE radius
        float totalAoeRadius = baseAoeRadius + data.bonusAoERadius;
        LayerMask layerToUse = enemyLayer.value != 0 ? enemyLayer : cachedEnemyLayer;
        Collider[] hits = Physics.OverlapSphere(impactPoint, totalAoeRadius, layerToUse);

        foreach (Collider enemy in hits)
        {
            if (enemy != null)
            {
                IDamageable health = enemy.GetComponent<IDamageable>();
                if (health != null)
                {
                    health.TakeDamage(data.attackDamage);
                }
            }
        }

        Debug.Log($"AoE Tower exploded with radius {totalAoeRadius}, hitting {hits.Length} enemies!");
    }

    public void PlayAttackSound()
    {
        if (attackSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(attackSound, 1f, true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(impactPoint, baseAoeRadius);
    }
}