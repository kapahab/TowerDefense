using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 lastKnownPosition;
    private float speed;

    // The NormalAttack script will call this right after spawning the projectile
    public void Setup(Transform newTarget, float moveSpeed)
    {
        target = newTarget;
        speed = moveSpeed;

        if (target != null)
        {
            lastKnownPosition = target.position;
        }
    }

    void Update()
    {
        // If the target hasn't been destroyed yet, update our homing position
        if (target != null)
        {
            lastKnownPosition = target.position;
        }

        // Move towards the target's center
        transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, speed * Time.deltaTime);

        // Make the projectile point in the direction it's flying (great for arrows/spears)
        if (transform.position != lastKnownPosition)
        {
            transform.LookAt(lastKnownPosition);
        }

        // When we get extremely close to the target position, destroy the visual projectile
        if (Vector3.Distance(transform.position, lastKnownPosition) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}