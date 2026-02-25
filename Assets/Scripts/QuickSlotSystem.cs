using System.Collections.Generic;
using UnityEngine;

public class QuickSlotSystem : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    // 4 slots, store itemName (null = empty)
    public List<string> quickSlots = new List<string>(4);

    private void Awake()
    {
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

    public void UseQuickSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= 4) return;

        string itemName = quickSlots[slotIndex];

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.Log($"Slot {slotIndex + 1} is empty.");
            return;
        }

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