using UnityEngine;
using UnityEngine.AI;

public class FriendFollowAssistAI : MonoBehaviour
{
    public float followDistance = 2.5f;
    public float assistTriggerRangeFromPlayer = 5f;

    [Header("Assist Attack")]
    public float attackCooldown = 1.0f;
    public int assistDamage = 10;

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

        // Always follow player
        float dToPlayer = Vector3.Distance(transform.position, player.position);
        if (dToPlayer > followDistance)
            agent.SetDestination(player.position);
        else if (agent.hasPath)
            agent.ResetPath();

        // Help attack when enemies are closer than 5 units from player
        Transform targetEnemy = FindEnemyNearPlayer();
        if (targetEnemy != null && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            AssistAttack(targetEnemy);
        }
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

        // For when EnemyHealth is implemented, try to damage it if present
        // enemy.GetComponent<EnemyHealth>()?.TakeDamage(assistDamage);
    }
}