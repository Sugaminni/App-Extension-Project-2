using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    public string itemName;
    public int quantity = 1;

    [Header("Pickup Settings")]
    public string playerTag = "Player";
    public bool destroyOnPickup = true;

    private InventorySystem inventorySystem;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();

        if (inventorySystem == null)
        {
            Debug.LogError("ItemPickup: InventorySystem not found in scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        TryPickup();
    }

    // Handles the logic of adding the item to inventory and destroying the pickup if successful
    private void TryPickup()
    {
        if (inventorySystem == null)
        {
            Debug.LogError("ItemPickup: Cannot pick up item because InventorySystem is missing.");
            return;
        }

        if (string.IsNullOrWhiteSpace(itemName))
        {
            Debug.LogWarning($"ItemPickup on '{gameObject.name}' has no itemName assigned.");
            return;
        }

        if (quantity <= 0)
        {
            Debug.LogWarning($"ItemPickup on '{gameObject.name}' has invalid quantity: {quantity}");
            return;
        }

        bool added = inventorySystem.AddItem(itemName, quantity);

        if (added)
        {
            Debug.Log($"Picked up {itemName} x{quantity}");

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log($"Could not pick up {itemName}. Inventory may be full.");
        }
    }
}