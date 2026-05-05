using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour, IAttackStrategy
{
    [Header("Visuals")]
    [Tooltip("The object the enemy throws (e.g., a rock or spear)")]
    public GameObject projectilePrefab;
    [Tooltip("Where the projectile spawns (e.g., the enemy's hand)")]
    public Transform firePoint;
    [Tooltip("How fast the projectile flies through the air")]
    public float projectileSpeed = 10f;

    [Header("Audio")]
    public AudioClip attackSound;

    private Transform target;

    public Transform ChooseTarget(List<Transform> potentialTargets)
    {
        if (potentialTargets != null && potentialTargets.Count > 0)
        {
            target = potentialTargets[0]; // Selects the closest tower
            return target;
        }
        return null;
    }

    public void ExecuteAttack(EnemyData data)
    {
        if (target == null) return;

        // 1. Play the attack sound immediately
        PlayAttackSound();

        // 2. Spawn the visual projectile and calculate flight time
        float timeToHit = 0f;

        if (projectilePrefab != null && firePoint != null)
        {
            // Spawn the dummy projectile
            GameObject visualProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Calculate how long it will take to hit based on distance and speed
            float distance = Vector3.Distance(firePoint.position, target.position);
            timeToHit = distance / projectileSpeed;

            // THE FIX: Grab the movement script and tell it to go!
            ProjectileMovement mover = visualProjectile.GetComponent<ProjectileMovement>();
            if (mover != null)
            {
                mover.Setup(target, projectileSpeed);
            }
            else
            {
                Debug.LogWarning("Your Projectile Prefab is missing the ProjectileMovement script!");
                // Fallback: Just delete it after the timer so it doesn't clutter the map forever
                Destroy(visualProjectile, timeToHit);
            }
        }

        // 3. Delay the actual damage so it syncs perfectly with the visual impact
        StartCoroutine(DealDamageAfterDelay(data.attackDamage, timeToHit));
    }

    private IEnumerator DealDamageAfterDelay(float damage, float delayTime)
    {
        // Wait while the projectile is in the air
        yield return new WaitForSeconds(delayTime);

        // CRITICAL CHECK: Make sure the target wasn't destroyed by another enemy while our projectile was mid-air!
        if (target != null)
        {
            IDamageable damageableTarget = target.GetComponent<IDamageable>();
            if (damageableTarget != null)
            {
                damageableTarget.TakeDamage(damage);
                Debug.DrawLine(transform.position, target.position, Color.red, 0.5f);
            }
        }
    }

    private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            // If you are using a global SoundManager (like SoundManager.Instance.PlaySFX), put that here.
            // Otherwise, this built-in Unity method plays the sound in 3D space:
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
    }
}