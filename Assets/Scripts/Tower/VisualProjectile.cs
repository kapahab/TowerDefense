using UnityEngine;

public class VisualProjectile : MonoBehaviour
{
    public float speed = 20f;
    private Transform target;
    private Vector3 lastKnownPosition;

    // The tower calls this just to say "Look pretty and fly over there"
    public void PlayVisual(Transform enemyTarget)
    {
        target = enemyTarget;
        if (target != null) lastKnownPosition = target.position;

        // Failsafe: Destroy this visual after 3 seconds no matter what, so they don't leak memory
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // If the enemy dies while the arrow is mid-air, it will just fly to where they used to be
        if (target != null)
        {
            // Aim at the chest, not the feet (optional tweak)
            lastKnownPosition = target.position + (Vector3.up * 0.5f);
        }

        // Fly forward
        transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, speed * Time.deltaTime);

        // Point the arrow tip exactly where it's going
        Vector3 direction = lastKnownPosition - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Once it reaches the destination, poof!
        if (Vector3.Distance(transform.position, lastKnownPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}