using System;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;

    // Constructor to initialize the inventory item with a name and quantity
    public InventoryItem(string name, int amount)
    {
        itemName = name;
        quantity = amount;
    }
}