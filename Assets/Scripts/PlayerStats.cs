using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int maxHealth = 100;
    public int currentHealth = 100;

    public float damageMultiplier = 1f;
    public float speed = 9f;
    public int kills = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        LoadStats();

        // Safety fix: do not allow the player to start dead from old bad saved data.
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            SaveStats();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        damageMultiplier = Mathf.Max(0f, damageMultiplier);
        speed = Mathf.Max(0f, speed);
    }

    public void AddKill()
    {
        kills++;
        SaveStats();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SaveStats();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        SaveStats();
    }

    public void SetDamage(float newDamage)
    {
        damageMultiplier = Mathf.Max(0f, newDamage);
        SaveStats();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Max(0f, newSpeed);
        SaveStats();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("Health", currentHealth);
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        PlayerPrefs.SetFloat("Damage", damageMultiplier);
        PlayerPrefs.SetFloat("Speed", speed);
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.Save();
    }

    public void LoadStats()
    {
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 100);
        currentHealth = PlayerPrefs.GetInt("Health", maxHealth);
        damageMultiplier = PlayerPrefs.GetFloat("Damage", 1f);
        speed = PlayerPrefs.GetFloat("Speed", 9f);
        kills = PlayerPrefs.GetInt("Kills", 0);
    }

    public void ResetStatsForNewGame()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        damageMultiplier = 1f;
        speed = 9f;
        kills = 0;

        SaveStats();

        Debug.Log("Player stats reset for new game.");
    }
}