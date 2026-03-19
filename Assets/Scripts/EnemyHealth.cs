using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // damage the enemy by a certain amount, without going below 0
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (currentHealth <= 0) return;
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (currentHealth == 0)
        {
            isDead = true;
            Debug.Log($"{name} died");

            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.AddKill();
            }

            gameObject.SetActive(false);
        }
    }
}