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
    private InventoryUIController inventoryUI;

    private void Start()
    {
        currentBulletPrefab = redBulletPrefab;
        inventoryUI = FindObjectOfType<InventoryUIController>();
    }

    private void Update()
    {
        if (inventoryUI != null && inventoryUI.IsOpen)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootCooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (currentBulletPrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(shootOffset);
        GameObject bullet = Instantiate(currentBulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = transform.forward;
            rb.linearVelocity = dir * bulletSpeed;
        }
    }

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
                break;
        }
    }
}