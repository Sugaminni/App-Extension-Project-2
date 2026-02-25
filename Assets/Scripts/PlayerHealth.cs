using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    // Heal the player by a certain amount, without exceeding max health
    public void Heal(int amount)
    {
        if (amount <= 0) return;

        int before = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        Debug.Log($"Healed {amount}. Health: {before} -> {currentHealth}");
    }

    // Damage the player by a certain amount, without going below 0
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        int before = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - amount);

        Debug.Log($"Took {amount} damage. Health: {before} -> {currentHealth}");
    }
}