using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour, ISlowable
{
    [Header("Movement Settings")]
    public float baseSpeed = 3f;
    private float currentSpeed;

    private bool isStopped = false;
    private bool isSlowed = false;

    private Transform[] path;
    private int currentWaypointIndex = 0;

    [Header("Overtaking & Spacing (Soft Collision)")]
    [Tooltip("How far away they push each other to slide past slow units")]
    public float separationRadius = 1f;
    public float separationForce = 4f;
    public LayerMask enemyLayer;

    [Header("Organic Variety")]
    [Tooltip("Shifts them slightly left/right so they don't walk in a perfect single-file line")]
    public float laneOffsetAmount = 0.15f;
    private Vector3 personalLaneOffset;

    [Header("Footstep Audio (Optimized)")]
    [Tooltip("Drag the AudioSource component attached to this enemy here")]
    public AudioSource footstepSource;
    [Tooltip("Drop 3 or 4 different footstep sounds here so they don't sound like a machine gun")]
    public AudioClip[] footstepClips;
    [Tooltip("How often the footstep sound plays in seconds")]
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    void Start()
    {
        currentSpeed = baseSpeed;

        personalLaneOffset = new Vector3(
            Random.Range(-laneOffsetAmount, laneOffsetAmount),
            0,
            Random.Range(-laneOffsetAmount, laneOffsetAmount)
        );
    }

    void Update()
    {
        if (isStopped || path == null || currentWaypointIndex >= path.Length) return;

        // 1. CALCULATE THE TARGET FIRST 
        Vector3 targetPos = path[currentWaypointIndex].position + personalLaneOffset;
        Vector3 exactCenter = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        // 2. FIND THE TRUE "SIDEWAYS" OF THE ROAD 
        Vector3 roadDirection = (exactCenter - transform.position).normalized;
        if (roadDirection == Vector3.zero) roadDirection = transform.forward;
        Vector3 roadRight = Vector3.Cross(Vector3.up, roadDirection).normalized;

        // --- 3. OVERTAKING / SIDEWAYS SLIDE ---
        Vector3 pushForce = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);

        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.gameObject != this.gameObject)
            {
                Vector3 directionAway = transform.position - neighbor.transform.position;
                directionAway.y = 0;
                float distance = directionAway.magnitude;

                if (distance < 0.001f)
                {
                    directionAway = roadRight * (Random.value > 0.5f ? 1f : -1f);
                    distance = 0.01f;
                }

                float pushPower = (separationRadius - distance) / separationRadius;

                float sidewaysDot = Vector3.Dot(directionAway.normalized, roadRight);
                Vector3 sidewaysPush = roadRight * sidewaysDot;

                pushForce += sidewaysPush * pushPower;
            }
        }

        transform.position += pushForce * separationForce * Time.deltaTime;

        // --- 4. FORWARD MOVEMENT ---
        transform.position = Vector3.MoveTowards(transform.position, exactCenter, currentSpeed * Time.deltaTime);

        Vector3 direction = exactCenter - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, exactCenter) <= 0.001f)
        {
            currentWaypointIndex++;
        }

        // --- 5. AUDIO FOOTSTEPS ---
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // Safety check: Do we have sounds and a speaker to play them from?
        if (footstepClips == null || footstepClips.Length == 0 || footstepSource == null) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            // Pick a completely random clip from the array
            AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];

            // Tweak the pitch and volume slightly every single step for maximum organic variety
            footstepSource.pitch = Random.Range(0.85f, 1.15f);
            footstepSource.volume = Random.Range(0.2f, 0.35f);

            // Play the sound through the existing component. ZERO instantiating!
            footstepSource.PlayOneShot(randomClip);

            float speedMultiplier = baseSpeed / currentSpeed;
            footstepTimer = footstepInterval * speedMultiplier;
        }
    }

    public void SetPath(Transform[] newPath, int startingIndex = 0)
    {
        path = newPath;
        isStopped = false;

        if (path != null && path.Length > 0)
        {
            currentWaypointIndex = startingIndex;
            if (currentWaypointIndex >= path.Length) currentWaypointIndex = path.Length - 1;
        }
    }

    public int GetCurrentWaypointIndex() { return currentWaypointIndex; }
    public void MoveTo(Transform targetTransform) { SetPath(new Transform[] { targetTransform }); }
    public void StopMoving() { isStopped = true; }

    public void Slow(float slowAmount, float slowDuration)
    {
        if (isSlowed) return;
        StartCoroutine(SlowCoroutine(slowAmount, slowDuration));
    }

    IEnumerator SlowCoroutine(float slowAmount, float slowDuration)
    {
        isSlowed = true;
        currentSpeed = baseSpeed * (1f - slowAmount);
        yield return new WaitForSeconds(slowDuration);
        currentSpeed = baseSpeed;
        isSlowed = false;
    }
}