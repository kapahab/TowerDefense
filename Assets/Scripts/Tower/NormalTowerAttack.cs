using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTowerAttack : MonoBehaviour, ITowerAttackStrategy
{
    [Header("Visuals")]
    public GameObject arrowVisualPrefab;

    public Transform firePoint;

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
        if (targetTransform != null && targetDamageable != null)
        {
            GameObject visualArrow = Instantiate(arrowVisualPrefab, firePoint.position, firePoint.rotation);
            VisualProjectile visualScript = visualArrow.GetComponent<VisualProjectile>();
            visualScript.PlayVisual(targetTransform);

            float distance = Vector3.Distance(firePoint.position, targetTransform.position);
            float timeToHit = distance / visualScript.speed;

            PlayAttackSound();
            StartCoroutine(DealDamageAfterDelay(data.attackDamage, timeToHit));
        }
    }

    private IEnumerator DealDamageAfterDelay(float damage, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (targetTransform != null)
        {
            targetDamageable.TakeDamage(damage);
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