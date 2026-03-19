using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int maxHealth = 100;
    public int currentHealth;

    public float damageMultiplier = 1f;
    public float speed = 9f;
    public int kills = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadStats();
    }

    public void AddKill()
    {
        kills++;
        SaveStats();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        SaveStats();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        SaveStats();
    }

    // Save stats to PlayerPrefs for persistence
    public void SaveStats()
    {
        PlayerPrefs.SetInt("Health", currentHealth);
        PlayerPrefs.SetFloat("Damage", damageMultiplier);
        PlayerPrefs.SetFloat("Speed", speed);
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.Save();
    }

    // Load stats from PlayerPrefs
    public void LoadStats()
    {
        currentHealth = PlayerPrefs.GetInt("Health", maxHealth);
        damageMultiplier = PlayerPrefs.GetFloat("Damage", 1f);
        speed = PlayerPrefs.GetFloat("Speed", 9f);
        kills = PlayerPrefs.GetInt("Kills", 0);
    }
}