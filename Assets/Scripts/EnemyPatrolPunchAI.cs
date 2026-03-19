using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolPunchAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float chaseRange = 10f;
    public float punchRange = 2f;

    [Header("Punch")]
    public float punchCooldown = 1.0f;
    public int punchDamage = 10;

    private NavMeshAgent agent;
    private Transform player;
    private int wpIndex;
    private float nextPunchTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Start()
    {
        GoToNextWaypoint();
    }

    private void Update()
    {
        if (player == null) return;
        if (agent == null) return;

        Vector3 flatEnemyPos = transform.position;
        Vector3 flatPlayerPos = player.position;
        flatEnemyPos.y = 0f;
        flatPlayerPos.y = 0f;

        float d = Vector3.Distance(flatEnemyPos, flatPlayerPos);

        if (d > chaseRange)
        {
            // Patrol > 10
            PatrolTick();
            return;
        }

        if (d <= punchRange)
        {
            // Punch <= 2
            if (agent.hasPath) agent.ResetPath();
            FaceTarget(player.position);

            if (Time.time >= nextPunchTime)
            {
                nextPunchTime = Time.time + punchCooldown;
                Punch();
            }
            return;
        }

        // Between 2 and 10: move toward player
        agent.SetDestination(player.position);
        FaceTarget(player.position);
    }

    private void PatrolTick()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (agent == null) return;

        // Move to next waypoint once close enough to the current one
        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
            GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (agent == null) return;

        agent.SetDestination(waypoints[wpIndex].position);
        wpIndex = (wpIndex + 1) % waypoints.Length;
    }

    private void Punch()
    {
        // Try to damage PlayerHealth if present
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(punchDamage);

        Debug.Log($"{name} punched player for {punchDamage}");
    }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
}