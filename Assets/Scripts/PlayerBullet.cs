using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Damage enemies on collision, and destroy the bullet. Ignore collisions with the player.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}