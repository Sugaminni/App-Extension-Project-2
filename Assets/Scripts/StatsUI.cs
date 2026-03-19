using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public TMP_Text statsText;
    public GameObject statsPanel;

    private void Start()
    {
        if (statsPanel != null)
            statsPanel.SetActive(false);

        UpdateStatsText();
    }

    private void Update()
    {
        // Toggle UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (statsPanel != null)
            {
                statsPanel.SetActive(!statsPanel.activeSelf);

                if (statsPanel.activeSelf)
                    UpdateStatsText();
            }
        }

        // only update stats if UI is active
        if (statsPanel == null || !statsPanel.activeSelf)
            return;

        UpdateStatsText();
    }

    // Update the stats text with current player stats
    private void UpdateStatsText()
    {
        if (statsText == null)
        {
            Debug.LogError("StatsUI: statsText is not assigned.");
            return;
        }

        if (PlayerStats.Instance == null)
        {
            Debug.LogError("StatsUI: PlayerStats.Instance is null.");
            statsText.text = "PlayerStats missing";
            return;
        }

        var stats = PlayerStats.Instance;

        statsText.text =
            "Health: " + stats.currentHealth + "\n" +
            "Damage: " + stats.damageMultiplier + "\n" +
            "Speed: " + stats.speed + "\n" +
            "Kills: " + stats.kills;
    }
}