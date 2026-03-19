using System.Collections.Generic;
using UnityEngine;
using System;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private const int MAX_UNIQUE_ITEMS = 12;

    // Event to notify UI / QuickSlot that inventory changed
    public event Action OnInventoryChanged;

    [Header("Debug")]
    [SerializeField] private bool debugFillInventory = true;

    // Adds Item
    public bool AddItem(string itemName, int amount = 1)
    {
        // basic validation
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Debug.LogWarning("Cannot add item with empty name.");
            return false;
        }

        if (amount <= 0)
        {
            Debug.Log("Amount must be greater than 0.");
            return false;
        }

        // Check if item already exists
        InventoryItem existingItem = inventory.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += amount;
            Debug.Log(itemName + " quantity increased to " + existingItem.quantity);
            OnInventoryChanged?.Invoke();
            return true;
        }
        else
        {
            if (inventory.Count >= MAX_UNIQUE_ITEMS)
            {
                Debug.Log("Storage Full! Cannot add more unique items.");
                return false;
            }

            inventory.Add(new InventoryItem(itemName, amount));
            Debug.Log(itemName + " added to inventory.");
            OnInventoryChanged?.Invoke();
            return true;
        }
    }

    // Use/Remove Item
    public bool UseItem(string itemName, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            Debug.LogWarning("Cannot use item with empty name.");
            return false;
        }

        if (amount <= 0)
        {
            Debug.Log("Amount must be greater than 0.");
            return false;
        }

        InventoryItem existingItem = inventory.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            if (existingItem.quantity < amount)
            {
                Debug.Log("Not enough quantity to use.");
                return false;
            }

            existingItem.quantity -= amount;

            if (existingItem.quantity <= 0)
            {
                inventory.Remove(existingItem);
                Debug.Log(itemName + " removed from inventory.");
            }
            else
            {
                Debug.Log(itemName + " quantity reduced to " + existingItem.quantity);
            }

            OnInventoryChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Item is not found in inventory.");
            return false;
        }
    }

    // Helper method 
    public bool HasItem(string itemName)
    {
        InventoryItem item = inventory.Find(i => i.itemName == itemName);
        return item != null && item.quantity > 0;
    }

    // Debug method to print inventory
    public void PrintInventory()
    {
        Debug.Log("------ INVENTORY ------");

        foreach (InventoryItem item in inventory)
        {
            Debug.Log(item.itemName + " | Quantity: " + item.quantity);
        }

        Debug.Log("-----------------------");
    }

    public int GetItemQuantity(string itemName)
    {
        foreach (var item in inventory)
        {
            if (item.itemName == itemName)
                return item.quantity;
        }

        return 0;
    }

    private void Start()
    {
        if (!debugFillInventory) return;

        // EXISTING ITEMS
        AddItem("Health10", 2);
        AddItem("Health20", 1);
        AddItem("Health30", 1);
        AddItem("RedBullet", 1);
        AddItem("GreenBullet", 1);
        AddItem("BlueBullet", 1);

        // NEW ITEMS (for testing)
        AddItem("Medkit", 1);
        AddItem("Bandage", 2);
        AddItem("SpeedBoost", 1);
        AddItem("DamageBoost", 1);
        AddItem("Shield", 1);
        AddItem("ArmorBoost", 1);
        AddItem("AmmoPack", 1);

        PrintInventory();
    }
}