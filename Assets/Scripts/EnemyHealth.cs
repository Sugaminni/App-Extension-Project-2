using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

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

        int before = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (currentHealth == 0)
        {
            Debug.Log($"{name} died");
            gameObject.SetActive(false);
        }
    }
}