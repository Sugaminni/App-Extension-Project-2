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

        float d = Vector3.Distance(transform.position, player.position);

        if (d > chaseRange)
        {
            // Patrol > 10
            PatrolTick();
            return;
        }

        if (d <= punchRange)
        {
            // Punch <= 2
            agent.ResetPath();
            FaceTarget(player.position);

            if (Time.time >= nextPunchTime)
            {
                nextPunchTime = Time.time + punchCooldown;
                Punch();
            }
            return;
        }

        // Between 2 and 10: keep patrolling
        PatrolTick();
    }

    private void PatrolTick()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        agent.SetDestination(waypoints[wpIndex].position);
        wpIndex = (wpIndex + 1) % waypoints.Length;
    }

    private void Punch()
    {
        // Testing: try to damage PlayerHealth if present
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