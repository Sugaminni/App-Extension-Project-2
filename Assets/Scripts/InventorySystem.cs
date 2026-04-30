using System.Collections.Generic;
using UnityEngine;
using System;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private const int MAX_UNIQUE_ITEMS = 12;

    public event Action OnInventoryChanged;

    [Header("Debug")]
    [SerializeField] private bool debugFillInventory = false;

    private void Start()
    {
        if (!debugFillInventory) return;

        AddItem("Health10", 2);
        AddItem("Health20", 1);
        AddItem("Health30", 1);
        AddItem("RedBullet", 1);
        AddItem("GreenBullet", 1);
        AddItem("BlueBullet", 1);

        AddItem("Medkit", 1);
        AddItem("Bandage", 2);
        AddItem("SpeedBoost", 1);
        AddItem("DamageBoost", 1);
        AddItem("Shield", 1);
        AddItem("ArmorBoost", 1);
        AddItem("AmmoPack", 1);

        PrintInventory();
    }

    public bool AddItem(string itemName, int amount = 1)
    {
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

        InventoryItem existingItem = inventory.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += amount;
            Debug.Log(itemName + " quantity increased to " + existingItem.quantity);
            OnInventoryChanged?.Invoke();
            return true;
        }

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

        if (existingItem == null)
        {
            Debug.Log("Item is not found in inventory.");
            return false;
        }

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

    public bool HasItem(string itemName)
    {
        InventoryItem item = inventory.Find(i => i.itemName == itemName);
        return item != null && item.quantity > 0;
    }

    public int GetItemQuantity(string itemName)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.itemName == itemName)
                return item.quantity;
        }

        return 0;
    }

    public void ClearInventory()
    {
        inventory.Clear();
        OnInventoryChanged?.Invoke();
        Debug.Log("Inventory cleared.");
    }

    public void PrintInventory()
    {
        Debug.Log("------ INVENTORY ------");

        foreach (InventoryItem item in inventory)
        {
            Debug.Log(item.itemName + " | Quantity: " + item.quantity);
        }

        Debug.Log("-----------------------");
    }

    // Converts current inventory into saveable data.
    public InventorySaveData GetSaveData()
    {
        InventorySaveData data = new InventorySaveData();

        foreach (InventoryItem item in inventory)
        {
            InventoryItemSaveData itemData = new InventoryItemSaveData();
            itemData.itemName = item.itemName;
            itemData.quantity = item.quantity;

            data.items.Add(itemData);
        }

        return data;
    }

    // Loads inventory from saved data.
    public void LoadFromSaveData(InventorySaveData data)
    {
        inventory.Clear();

        if (data != null && data.items != null)
        {
            foreach (InventoryItemSaveData itemData in data.items)
            {
                if (!string.IsNullOrWhiteSpace(itemData.itemName) && itemData.quantity > 0)
                {
                    inventory.Add(new InventoryItem(itemData.itemName, itemData.quantity));
                }
            }
        }

        OnInventoryChanged?.Invoke();
        Debug.Log("Inventory loaded from save.");
        PrintInventory();
    }

    public void LoadDefaultNewGameInventory()
    {
        inventory.Clear();

        AddItem("Health10", 3);
        AddItem("Health20", 2);
        AddItem("Health30", 1);

        AddItem("RedBullet", 10);
        AddItem("GreenBullet", 8);
        AddItem("BlueBullet", 5);

        AddItem("Medkit", 2);
        AddItem("Bandage", 4);
        AddItem("SpeedBoost", 1);
        AddItem("DamageBoost", 1);
        AddItem("Shield", 1);
        AddItem("AmmoPack", 3);

        OnInventoryChanged?.Invoke();

        Debug.Log("Default new game inventory loaded.");
        PrintInventory();
    }
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemSaveData> items = new List<InventoryItemSaveData>();
}

[System.Serializable]
public class InventoryItemSaveData
{
    public string itemName;
    public int quantity;
}