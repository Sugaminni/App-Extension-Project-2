using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float lifeSeconds = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet hits the player, damage them
        PlayerHealth ph = collision.collider.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(5);

        Destroy(gameObject);
    }
}