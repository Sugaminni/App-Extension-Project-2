using System.Collections.Generic;
using UnityEngine;

public class QuickSlotSystem : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private PlayerHealth playerHealth;

    // 4 slots, store itemName (null = empty)
    public List<string> quickSlots = new List<string>(4);

    private void Awake()
    {
        // Auto-wire references so AssignToSlot() doesn't throw NullReferenceException
        if (inventorySystem == null)
            inventorySystem = FindObjectOfType<InventorySystem>();

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (inventorySystem == null)
            Debug.LogError("QuickSlotSystem: InventorySystem not found in scene.");

        if (playerHealth == null)
            Debug.LogError("QuickSlotSystem: PlayerHealth not found in scene.");

        // Ensures 4 slots exist
        if (quickSlots.Count != 4)
        {
            quickSlots.Clear();
            for (int i = 0; i < 4; i++) quickSlots.Add(null);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseQuickSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseQuickSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseQuickSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseQuickSlot(3);
    }

    public void AssignToSlot(int slotIndex, string itemName)
    {
        if (slotIndex < 0 || slotIndex >= 4) return;

        if (inventorySystem == null)
        {
            Debug.LogError("AssignToSlot failed: inventorySystem is null.");
            return;
        }

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.Log("Cannot assign empty itemName.");
            return;
        }

        // Only allow assigning if the item exists in inventory
        bool exists = inventorySystem.inventory.Exists(i => i.itemName == itemName);
        if (!exists)
        {
            Debug.Log($"Cannot assign '{itemName}' — not in inventory.");
            return;
        }

        quickSlots[slotIndex] = itemName;
        Debug.Log($"Assigned '{itemName}' to slot {slotIndex + 1}");
    }

    private void ApplyItemEffect(string itemName)
    {
        if (playerHealth == null)
        {
            Debug.LogError("ApplyItemEffect failed: playerHealth is null.");
            return;
        }

        switch (itemName)
        {
            case "Health10":
                playerHealth.Heal(10);
                break;
            case "Health20":
                playerHealth.Heal(20);
                break;
            case "Health30":
                playerHealth.Heal(30);
                break;
            default:
                Debug.Log($"Used item: {itemName} (no effect hooked up yet)");
                break;
        }
    }

    public void UseQuickSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= 4) return;

        if (inventorySystem == null)
        {
            Debug.LogError("UseQuickSlot failed: inventorySystem is null.");
            return;
        }

        string itemName = quickSlots[slotIndex];

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.Log($"Slot {slotIndex + 1} is empty.");
            return;
        }

        // If the item no longer exists in inventory, clear the slot
        bool exists = inventorySystem.inventory.Exists(i => i.itemName == itemName);
        if (!exists)
        {
            quickSlots[slotIndex] = null;
            Debug.Log($"'{itemName}' depleted — cleared slot {slotIndex + 1}");
            return;
        }

        ApplyItemEffect(itemName);
        inventorySystem.UseItem(itemName);
        inventorySystem.PrintInventory();

        // If the item no longer exists in inventory, clear the slot
        bool stillExists = inventorySystem.inventory.Exists(i => i.itemName == itemName);
        if (!stillExists)
        {
            quickSlots[slotIndex] = null;
            Debug.Log($"'{itemName}' depleted — cleared slot {slotIndex + 1}");
        }
    }

    private void Start()
    {
        // Temp test
        AssignToSlot(0, "Health10");     // key 1
        AssignToSlot(1, "RedBullet");    // key 2
        AssignToSlot(2, "GreenBullet");  // key 3
        AssignToSlot(3, "BlueBullet");   // key 4
    }
}