using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet Types")]
    public GameObject redBulletPrefab;
    public GameObject greenBulletPrefab;
    public GameObject blueBulletPrefab;

    [Header("Shooting")]
    public float bulletSpeed = 20f;
    public float shootCooldown = 0.3f;
    public Vector3 shootOffset = new Vector3(0f, -0.8f, 0.5f);

    private GameObject currentBulletPrefab;
    private float nextShootTime;

    private void Start()
    {
        // Default bullet type
        currentBulletPrefab = redBulletPrefab;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.damageMultiplier = GetBulletDamage("RedBullet");
            PlayerStats.Instance.SaveStats();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootCooldown;
            Shoot();
        }
    }

    // Simple shoot method that spawns a bullet and gives it velocity
    private void Shoot()
    {
        if (currentBulletPrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(shootOffset);
        GameObject bullet = Instantiate(currentBulletPrefab, spawnPos, Quaternion.identity);

        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();
        if (pb != null && PlayerStats.Instance != null)
        {
            pb.damage = Mathf.RoundToInt(PlayerStats.Instance.damageMultiplier);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = transform.forward;
            rb.linearVelocity = dir * bulletSpeed;
        }
    }

    // Switch the currently equipped bullet type
    public void SetBulletType(string bulletName)
    {
        switch (bulletName)
        {
            case "RedBullet":
                currentBulletPrefab = redBulletPrefab;
                Debug.Log("Equipped RedBullet");
                break;

            case "GreenBullet":
                currentBulletPrefab = greenBulletPrefab;
                Debug.Log("Equipped GreenBullet");
                break;

            case "BlueBullet":
                currentBulletPrefab = blueBulletPrefab;
                Debug.Log("Equipped BlueBullet");
                break;

            default:
                Debug.Log($"Unknown bullet type: {bulletName}");
                return;
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.damageMultiplier = GetBulletDamage(bulletName);
            PlayerStats.Instance.SaveStats();
        }
    }

    private float GetBulletDamage(string bulletName)
    {
        switch (bulletName)
        {
            case "RedBullet":
                return 10f;
            case "GreenBullet":
                return 20f;
            case "BlueBullet":
                return 30f;
            default:
                return 10f;
        }
    }
}