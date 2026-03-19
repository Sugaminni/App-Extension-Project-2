using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // When the bullet collides with another collider, check if it's the player and apply damage, then destroy the bullet
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}