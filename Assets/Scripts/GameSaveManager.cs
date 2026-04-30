using UnityEngine;
using System.IO;

public class GameSaveManager : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public Transform spawnPoint;

    [Header("Inventory")]
    public InventorySystem inventorySystem;

    [Header("Optional Player Stats")]
    public PlayerStats playerStats;

    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savegame.json";
    }

    public bool HasSaveFile()
    {
        return File.Exists(savePath);
    }

    public void NewGame()
    {
        Debug.Log("Starting NEW GAME");

        // Delete the old resume file.
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Old save deleted: " + savePath);
        }

        // Reset player position.
        ResetPlayerPosition();

        // Reset inventory to default new-game inventory.
        if (inventorySystem != null)
        {
            inventorySystem.ClearInventory();
            inventorySystem.LoadDefaultNewGameInventory();
        }
        else
        {
            Debug.LogWarning("InventorySystem is not assigned.");
        }

        // Optional: reset player stats if assigned.
        if (playerStats != null)
        {
            playerStats.ResetStatsForNewGame();
        }

        // Create a fresh save file for this new game.
        SaveGame();

        Debug.Log("New game created.");
    }

    public void ResumeGame()
    {
        Debug.Log("Trying to RESUME GAME");

        if (File.Exists(savePath))
        {
            LoadGame();
            Debug.Log("Resume successful.");
        }
        else
        {
            Debug.LogWarning("No save file found. Starting new game instead.");
            NewGame();
        }
    }

    public void SaveGame()
    {
        if (player == null)
        {
            Debug.LogWarning("Cannot save. Player is not assigned.");
            return;
        }

        SaveData data = new SaveData();

        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        data.playerRotX = player.eulerAngles.x;
        data.playerRotY = player.eulerAngles.y;
        data.playerRotZ = player.eulerAngles.z;

        if (inventorySystem != null)
        {
            data.inventory = inventorySystem.GetSaveData();
        }

        if (playerStats != null)
        {
            data.health = playerStats.currentHealth;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file exists.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Cannot load. Player is not assigned.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        player.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        player.eulerAngles = new Vector3(data.playerRotX, data.playerRotY, data.playerRotZ);

        if (inventorySystem != null)
        {
            inventorySystem.LoadFromSaveData(data.inventory);
        }

        if (playerStats != null)
        {
            playerStats.currentHealth = data.health;
            playerStats.SaveStats();
        }

        Debug.Log("Game loaded from: " + savePath);
    }

    private void ResetPlayerPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("Cannot reset player. Player is not assigned.");
            return;
        }

        if (spawnPoint != null)
        {
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
        }
        else
        {
            player.position = Vector3.zero;
            player.rotation = Quaternion.identity;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}

[System.Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;
    public float playerZ;

    public float playerRotX;
    public float playerRotY;
    public float playerRotZ;
    public InventorySaveData inventory;
    public int health = 100;
}