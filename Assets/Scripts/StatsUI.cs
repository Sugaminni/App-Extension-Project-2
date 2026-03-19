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
    }

    private void Update()
    {
        // Toggle UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statsPanel.SetActive(!statsPanel.activeSelf);
        }

        // only update stats if UI is active
        if (!statsPanel.activeSelf)
            return;

        var stats = PlayerStats.Instance;

        if (stats == null || statsText == null)
            return;

        statsText.text =
            "Health: " + stats.currentHealth + "\n" +
            "Damage: " + stats.damageMultiplier + "\n" +
            "Speed: " + stats.speed + "\n" +
            "Kills: " + stats.kills;
    }
}