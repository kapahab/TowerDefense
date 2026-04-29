using UnityEngine;

public class ArcVisualsProjectile : MonoBehaviour
{
    public float speed = 10f;
    [Tooltip("How high the projectile arcs into the air")]
    public float arcHeight = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float journeyLength;
    private float startTime;

    // We pass a fixed coordinate on the floor, not a moving enemy!
    public void PlayVisual(Vector3 targetGroundPos)
    {
        startPosition = transform.position;
        targetPosition = targetGroundPos;

        journeyLength = Vector3.Distance(startPosition, targetPosition);
        startTime = Time.time;

        // Failsafe memory cleanup
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (journeyLength <= 0) return;

        // 1. Calculate how far along the path we are (0.0 to 1.0)
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // 2. Move in a straight flat line from Start to Target
        Vector3 currentFlatPos = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

        // 3. Add the Arc! (Using a Sine wave to push it up in the middle of the journey)
        float currentHeight = Mathf.Sin(fractionOfJourney * Mathf.PI) * arcHeight;
        currentFlatPos.y += currentHeight;

        transform.position = currentFlatPos;

        // 4. Calculate the literal next millimeter of movement so we can point the nose of the projectile down as it falls
        if (fractionOfJourney < 0.95f)
        {
            Vector3 nextFlatPos = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney + 0.01f);
            nextFlatPos.y += Mathf.Sin((fractionOfJourney + 0.01f) * Mathf.PI) * arcHeight;

            Vector3 direction = nextFlatPos - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // 5. Delete when it hits the dirt
        if (fractionOfJourney >= 1f)
        {
            Destroy(gameObject);
        }
    }
}