using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("Health Source")]
    public PlayerHealth playerHealth;

    [Header("UI")]
    public RectTransform healthFill;
    public TMP_Text healthText;

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (playerHealth == null || healthFill == null)
            return;

        int currentHealth = Mathf.Clamp(playerHealth.CurrentHealth, 0, playerHealth.MaxHealth);
        int maxHealth = playerHealth.MaxHealth;

        float healthPercent = (float)currentHealth / maxHealth;

        healthFill.localScale = new Vector3(healthPercent, 1f, 1f);

        if (healthText != null)
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
    }
}