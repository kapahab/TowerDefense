using System.Collections;
using UnityEngine;

public class AreaTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("Visuals")]
    [Tooltip("The boulder or bomb that flies through the air")]
    public GameObject arcProjectilePrefab;
    [Tooltip("The explosion or crater left on the ground")]
    public GameObject explosionCraterPrefab;
    public Transform firePoint;

    [Header("Audio")]
    public AudioClip attackSound;

    private Collider[] targetsToHit;
    private Vector3 impactPoint;

    public void ChooseTarget(Collider[] potentialTargets)
    {
        targetsToHit = potentialTargets;

        // Aim exactly at the feet of the first enemy in the cluster
        if (potentialTargets.Length > 0 && potentialTargets[0] != null)
        {
            impactPoint = potentialTargets[0].transform.position;
            // Lock the Y axis to the floor so the math is perfect
            impactPoint.y = 0f;
        }
    }

    public void ExecuteAttack(TowerDataInstance data)
    {
        if (targetsToHit == null || targetsToHit.Length == 0) return;

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
        StartCoroutine(ApplyAoEAfterDelay(flightTime, data.attackDamage));
    }

    private IEnumerator ApplyAoEAfterDelay(float delayTime, float damage)
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

        // 2. Deal Damage to everyone in the cached array
        foreach (Collider enemy in targetsToHit)
        {
            // CRITICAL CHECK: The enemy might have been killed by another tower while our bomb was mid-air!
            if (enemy != null)
            {
                IDamageable health = enemy.GetComponent<IDamageable>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }

        Debug.Log($"AoE Tower exploded, hitting up to {targetsToHit.Length} enemies!");
    }

    public void PlayAttackSound()
    {
        if (attackSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(attackSound, 1f, true);
        }
    }
}