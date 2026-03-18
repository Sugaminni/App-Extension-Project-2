using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootCooldown = 0.3f;
    public Vector3 shootOffset = new Vector3(0f, -0.8f, 0.5f);

    private float nextShootTime;

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
        if (bulletPrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(shootOffset);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = transform.forward;
            rb.linearVelocity = dir * bulletSpeed;
        }
    }
}