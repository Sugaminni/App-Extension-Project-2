using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public InventorySlotUI slotPrefab;

    private InventorySystem inventorySystem;
    private readonly List<InventorySlotUI> spawnedSlots = new List<InventorySlotUI>();

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        else
            Debug.LogError("InventoryUIController: inventoryPanel not assigned.");

        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged += Refresh;
        else
            Debug.LogError("InventoryUIController: InventorySystem not found.");
    }

    private void OnDestroy()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged -= Refresh;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I detected");

            if (inventoryPanel == null)
            {
                Debug.Log("inventoryPanel is NULL");
                return;
            }

            bool current = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!current);

            Debug.Log("Panel active now: " + inventoryPanel.activeSelf);

            if (!current)
                Refresh();
        }
    }

    public void Refresh()
    {
        if (inventorySystem == null || slotsParent == null || slotPrefab == null) return;

        int uniqueCount = inventorySystem.inventory.Count;

        // Determine how many slots to show based on unique item count  
        int slotCount = (uniqueCount <= 6) ? 6 : uniqueCount;

        slotCount = Mathf.Clamp(slotCount, 6, 12);

        EnsureSlotObjects(slotCount);

        // Fill slots with items in order
        for (int i = 0; i < slotCount; i++)
        {
            if (i < uniqueCount)
            {
                var item = inventorySystem.inventory[i];
                spawnedSlots[i].SetItem(item.itemName, item.quantity);
            }
            else
            {
                spawnedSlots[i].SetEmpty();
            }
        }
    }

    private void EnsureSlotObjects(int needed)
    {
        // spawn more
        while (spawnedSlots.Count < needed)
        {
            var slot = Instantiate(slotPrefab, slotsParent);
            spawnedSlots.Add(slot);
        }

        // delete extra
        while (spawnedSlots.Count > needed)
        {
            var last = spawnedSlots[spawnedSlots.Count - 1];
            spawnedSlots.RemoveAt(spawnedSlots.Count - 1);
            Destroy(last.gameObject);
        }
    }
}