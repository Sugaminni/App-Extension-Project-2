using System;
using UnityEngine;

public class QuickSlotSystem : MonoBehaviour
{
    public event Action OnQuickSlotsChanged;

    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShoot playerShoot;

    private string[] quickSlots = new string[4];

    public int SelectedSlotIndex = 0;

    private void Awake()
    {
        if (inventorySystem == null)
            inventorySystem = FindObjectOfType<InventorySystem>();

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerShoot == null)
            playerShoot = FindObjectOfType<PlayerShoot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseQuickSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseQuickSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseQuickSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseQuickSlot(3);
    }

    // Assignment and selection methods for quick slots, called from InventoryUIController when dragging items to quick slots or clicking on them to select. Also called from QuickSlotUI when clicking on quick slot buttons.

    public void AssignToSlot(int slotIndex, string itemName)
    {
        if (slotIndex < 0 || slotIndex >= quickSlots.Length)
            return;

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("Cannot assign empty item.");
            return;
        }

        if (inventorySystem == null || !inventorySystem.HasItem(itemName))
        {
            Debug.LogWarning("Cannot assign item because it is not in inventory: " + itemName);
            return;
        }

        quickSlots[slotIndex] = itemName;

        Debug.Log("Assigned " + itemName + " to quick slot " + (slotIndex + 1));

        OnQuickSlotsChanged?.Invoke();
    }

    public void AssignToSelectedSlot(string itemName)
    {
        AssignToSlot(SelectedSlotIndex, itemName);
    }

    public void SelectSlot(int index)
    {
        SelectedSlotIndex = Mathf.Clamp(index, 0, 3);
    }

    public void SelectSlot1() => SelectSlot(0);
    public void SelectSlot2() => SelectSlot(1);
    public void SelectSlot3() => SelectSlot(2);
    public void SelectSlot4() => SelectSlot(3);

    // Use the item in the specified quick slot, applying its effect and consuming it if applicable. If the item is missing from inventory, clear the slot.

    public void UseSlot1() => UseQuickSlot(0);
    public void UseSlot2() => UseQuickSlot(1);
    public void UseSlot3() => UseQuickSlot(2);
    public void UseSlot4() => UseQuickSlot(3);

    public void UseQuickSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= quickSlots.Length)
            return;

        string itemName = quickSlots[slotIndex];

        if (string.IsNullOrEmpty(itemName))
            return;

        if (inventorySystem == null || !inventorySystem.HasItem(itemName))
        {
            quickSlots[slotIndex] = null;
            OnQuickSlotsChanged?.Invoke();
            return;
        }

        ApplyItemEffect(itemName);

        if (IsConsumable(itemName))
        {
            inventorySystem.UseItem(itemName, 1);

            if (!inventorySystem.HasItem(itemName))
                quickSlots[slotIndex] = null;
        }

        OnQuickSlotsChanged?.Invoke();
    }

    // Effects of using items - healing, changing bullet type, etc. Extend as needed for new items

    private void ApplyItemEffect(string itemName)
    {
        if (playerHealth != null)
        {
            if (itemName == "Health10") playerHealth.Heal(10);
            else if (itemName == "Health20") playerHealth.Heal(20);
            else if (itemName == "Health30") playerHealth.Heal(30);
        }

        if (playerShoot != null)
        {
            if (itemName == "RedBullet") playerShoot.SetBulletType("RedBullet");
            else if (itemName == "GreenBullet") playerShoot.SetBulletType("GreenBullet");
            else if (itemName == "BlueBullet") playerShoot.SetBulletType("BlueBullet");
        }
    }

    private bool IsConsumable(string itemName)
    {
        return itemName == "Health10"
            || itemName == "Health20"
            || itemName == "Health30";
    }

    // Ui helpers for QuickSlotUI to display item name and quantity

    public string GetItemInSlot(int index)
    {
        if (index < 0 || index >= quickSlots.Length)
            return null;

        return quickSlots[index];
    }

    public int GetQuantityInSlot(int index)
    {
        if (inventorySystem == null) return 0;

        string item = GetItemInSlot(index);
        if (string.IsNullOrEmpty(item)) return 0;

        return inventorySystem.GetItemQuantity(item);
    }
}