using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        if (PlayerStats.Instance != null)
        {
            maxHealth = PlayerStats.Instance.maxHealth;
            currentHealth = PlayerStats.Instance.currentHealth;

            if (currentHealth <= 0)
            {
                currentHealth = maxHealth;
                PlayerStats.Instance.currentHealth = currentHealth;
                PlayerStats.Instance.SaveStats();
            }
        }
        else
        {
            currentHealth = maxHealth;
        }
    }

    // Heal the player by a certain amount, without exceeding max health
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (currentHealth <= 0) return;

        int before = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        // Sync with PlayerStats
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.currentHealth = currentHealth;
            PlayerStats.Instance.SaveStats();
        }
    }

    // Damage the player by a certain amount, without going below 0
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (currentHealth <= 0) return;

        int before = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - amount);

        // Sync with PlayerStats
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.currentHealth = currentHealth;
            PlayerStats.Instance.SaveStats();
        }

        if (currentHealth == 0)
        {
            Debug.Log("Player died");
            gameObject.SetActive(false);
        }
    }
}