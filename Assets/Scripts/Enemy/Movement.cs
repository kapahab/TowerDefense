//using System.Collections;
//using UnityEngine;
//using UnityEngine.AI;

//public class Movement : MonoBehaviour, ISlowable
//{
//    [SerializeField] NavMeshAgent agent;
//    bool isSlowed = false;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    { }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void MoveTo(Transform targetTransform)
//    {
//        agent.isStopped = false;
//        agent.SetDestination(targetTransform.position);
//    }

//    public void StopMoving()
//    {
//        agent.isStopped = true;
//    }

//    public void Slow(float slowAmount, float slowDuration)
//    {
//        if (isSlowed) return; // Prevent stacking slows
//        StartCoroutine(SlowCoroutine(slowAmount, slowDuration));
//    }

//    IEnumerator SlowCoroutine(float slowAmount, float slowDuration)
//    {
//        isSlowed = true;
//        float originalSpeed = agent.speed;
//        agent.speed *= (1 - slowAmount);
//        yield return new WaitForSeconds(slowDuration);
//        agent.speed = originalSpeed;
//        isSlowed = false;
//    }
//}
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

    void Start()
    {
        // NO MORE SPEED VARIANCE! Everyone walks at the exact same pace.
        currentSpeed = baseSpeed;

        // We keep the lane offset so they still look like an organic crowd
        personalLaneOffset = new Vector3(
            Random.Range(-laneOffsetAmount, laneOffsetAmount),
            0,
            Random.Range(-laneOffsetAmount, laneOffsetAmount)
        );
    }

    void Update()
    {
        if (isStopped || path == null || currentWaypointIndex >= path.Length) return;

        // --- 1. OVERTAKING / SIDEWAYS SLIDE ---
        Vector3 pushForce = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);

        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.gameObject != this.gameObject)
            {
                Vector3 directionAway = transform.position - neighbor.transform.position;
                directionAway.y = 0;
                float distance = directionAway.magnitude;

                // Anti-stuck logic for Matryoshka babies spawning on exact same pixel
                if (distance < 0.001f)
                {
                    directionAway = transform.right * (Random.value > 0.5f ? 1f : -1f);
                    distance = 0.01f;
                }

                float pushPower = (separationRadius - distance) / separationRadius;

                // Only allow them to slide left and right (never forward/backward)
                float sidewaysDot = Vector3.Dot(directionAway.normalized, transform.right);
                Vector3 sidewaysPush = transform.right * sidewaysDot;

                pushForce += sidewaysPush * pushPower;
            }
        }

        // Apply the slide
        transform.position += pushForce * separationForce * Time.deltaTime;


        // --- 2. FORWARD MOVEMENT ---
        Vector3 targetPos = path[currentWaypointIndex].position + personalLaneOffset;
        Vector3 exactCenter = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        // Move forward at exactly currentSpeed
        transform.position = Vector3.MoveTowards(transform.position, exactCenter, currentSpeed * Time.deltaTime);

        // Snap rotation to face exactly where we are walking
        Vector3 direction = exactCenter - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, exactCenter) <= 0.001f)
        {
            currentWaypointIndex++;
        }
    }

    // --- YOUR HELPERS AND SLOW MECHANICS (Untouched) ---
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