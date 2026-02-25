using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private const int MAX_UNIQUE_ITEMS = 12;

    // Adds Item
    public void AddItem(string itemName, int amount = 1)
    {
        // Check if item already exists
        InventoryItem existingItem = inventory.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += amount;
            Debug.Log(itemName + " quantity increased to " + existingItem.quantity);
        }
        else
        {
            if (inventory.Count >= MAX_UNIQUE_ITEMS)
            {
                Debug.Log("Storage Full! Cannot add more unique items.");
                return;
            }

            inventory.Add(new InventoryItem(itemName, amount));
            Debug.Log(itemName + " added to inventory.");
        }
    }

    // Use/Remove Item
    public void UseItem(string itemName, int amount = 1)
    {
        InventoryItem existingItem = inventory.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
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
        }
        else
        {
            Debug.Log("Item is not found in inventory.");
        }
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

    private void Start()
    {
        AddItem("Health10", 2);
        AddItem("RedBullet", 3);
        AddItem("GreenBullet", 1);
        AddItem("BlueBullet", 1);

        PrintInventory();
    }
}