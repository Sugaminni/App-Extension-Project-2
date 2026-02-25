using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseShootAI : MonoBehaviour
{
    public float chaseRange = 10f;
    public float shootRange = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;           // placeholder projectile
    public Transform shootPoint;             
    public float bulletSpeed = 20f;
    public float shootCooldown = 1.0f;

    private NavMeshAgent agent;
    private Transform player;
    private float nextShootTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float d = Vector3.Distance(transform.position, player.position);

        if (d > chaseRange)
        {
            // Idle > 10
            if (agent.hasPath) agent.ResetPath();
            return;
        }

        if (d > shootRange)
        {
            // Chase <= 10 and > 5
            agent.SetDestination(player.position);
            return;
        }

        // Shoot <= 5
        agent.ResetPath();
        FaceTarget(player.position);

        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootCooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = shootPoint.forward * bulletSpeed;

        // Debug message for testing
        Debug.Log($"{name} shot at player");
    }

    private void FaceTarget(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
}