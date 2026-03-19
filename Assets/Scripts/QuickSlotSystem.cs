using System;
using UnityEngine;

public class QuickSlotSystem : MonoBehaviour
{
    public event Action OnQuickSlotsChanged;

    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShoot playerShoot;

    private string[] quickSlots = new string[4];
    private int selectedSlotIndex = 0;

    public int SelectedSlotIndex => selectedSlotIndex;

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

    public void SelectSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        Debug.Log("Selected quick slot: " + (slotIndex + 1));
    }

    public void AssignToSelectedSlot(string itemName)
    {
        quickSlots[selectedSlotIndex] = itemName;
        Debug.Log($"Assigned {itemName} to quick slot {selectedSlotIndex + 1}");
        OnQuickSlotsChanged?.Invoke();
    }

    public void UseQuickSlot(int slotIndex)
    {
        string itemName = quickSlots[slotIndex];

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.Log("Slot empty");
            return;
        }

        if (inventorySystem == null || !inventorySystem.HasItem(itemName))
        {
            quickSlots[slotIndex] = null;
            OnQuickSlotsChanged?.Invoke();
            return;
        }

        // Apply effect first
        ApplyItemEffect(itemName);

        // Only consume actual consumables
        if (IsConsumable(itemName))
        {
            inventorySystem.UseItem(itemName, 1);

            if (!inventorySystem.HasItem(itemName))
            {
                quickSlots[slotIndex] = null;
            }
        }

        OnQuickSlotsChanged?.Invoke();
    }

    private void ApplyItemEffect(string itemName)
    {
        if (itemName == "Health10") playerHealth.Heal(10);
        else if (itemName == "Health20") playerHealth.Heal(20);
        else if (itemName == "Health30") playerHealth.Heal(30);
        else if (itemName == "RedBullet" && playerShoot != null) playerShoot.SetBulletType("RedBullet");
        else if (itemName == "GreenBullet" && playerShoot != null) playerShoot.SetBulletType("GreenBullet");
        else if (itemName == "BlueBullet" && playerShoot != null) playerShoot.SetBulletType("BlueBullet");
    }

    private bool IsConsumable(string itemName)
    {
        return itemName == "Health10"
            || itemName == "Health20"
            || itemName == "Health30";
    }

    public string GetItemInSlot(int index)
    {
        return quickSlots[index];
    }

    public int GetQuantityInSlot(int index)
    {
        if (inventorySystem == null) return 0;

        string item = quickSlots[index];
        if (string.IsNullOrEmpty(item)) return 0;

        return inventorySystem.GetItemQuantity(item);
    }
}