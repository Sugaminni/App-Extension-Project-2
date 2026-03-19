using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FriendFollowAssistAI : MonoBehaviour
{
    public float followDistance = 2.5f;
    public float assistTriggerRangeFromPlayer = 5f;
    public float attackDistance = 4f;

    [Header("Assist Attack")]
    public float attackCooldown = 1.0f;
    public int assistDamage = 10;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Vector3 shootOffset = new Vector3(0f, 0.5f, 0.8f);

    private NavMeshAgent agent;
    private Transform player;
    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;
        if (agent == null) return;

        // Help attack when enemies are closer than 5 units from player
        Transform targetEnemy = FindEnemyNearPlayer();

        if (targetEnemy != null)
        {
            float dToEnemy = Vector3.Distance(transform.position, targetEnemy.position);

            if (dToEnemy > attackDistance)
            {
                agent.SetDestination(targetEnemy.position);
            }
            else
            {
                if (agent.hasPath)
                    agent.ResetPath();

                FaceTarget(targetEnemy.position);

                if (Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + attackCooldown;
                    AssistAttack(targetEnemy);
                }
            }

            return;
        }

        // Always follow player
        float dToPlayer = Vector3.Distance(transform.position, player.position);
        if (dToPlayer > followDistance)
            agent.SetDestination(player.position);
        else if (agent.hasPath)
            agent.ResetPath();
    }

    private Transform FindEnemyNearPlayer()
    {
        // Finds the closest enemy to the player within assistTriggerRangeFromPlayer, or null if none
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null || enemies.Length == 0) return null;

        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            if (e == null || !e.activeInHierarchy) continue;

            float d = Vector3.Distance(player.position, e.transform.position);
            if (d <= assistTriggerRangeFromPlayer && d < bestDist)
            {
                bestDist = d;
                best = e.transform;
            }
        }

        return best;
    }

    private void AssistAttack(Transform enemy)
    {
        // Debug message for testing
        Debug.Log($"{name} assisted attack on {enemy.name}");


        if (bulletPrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(shootOffset);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 target = enemy.position;
            target.y = spawnPos.y;

            Vector3 dir = (target - spawnPos).normalized;
            rb.linearVelocity = dir * bulletSpeed;
        }
    }

    // Rotate to face the target position, ignoring vertical difference
    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            10f * Time.deltaTime
        );
    }
}