using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] inventorySlots;

    private InventorySystem inventorySystem;
    private QuickSlotSystem quickSlotSystem;

    public bool IsOpen => inventoryPanel != null && inventoryPanel.activeSelf;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        quickSlotSystem = FindObjectOfType<QuickSlotSystem>();

        if (inventoryPanel == null)
            Debug.LogError("InventoryUIController: inventoryPanel not assigned.");

        if (inventorySystem == null)
            Debug.LogError("InventoryUIController: InventorySystem not found.");

        if (quickSlotSystem == null)
            Debug.LogError("InventoryUIController: QuickSlotSystem not found.");

        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged += RefreshInventoryUI;
    }

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        SetCursorState(false);
        RefreshInventoryUI();
    }

    private void OnDestroy()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged -= RefreshInventoryUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Toggle the inventory panel and cursor state, and refresh UI if opening
    public void ToggleInventory()
    {
        if (inventoryPanel == null)
            return;

        bool nextState = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(nextState);
        SetCursorState(nextState);

        if (nextState)
            RefreshInventoryUI();
    }

    private void SetCursorState(bool inventoryOpen)
    {
        Cursor.visible = inventoryOpen;
        Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Called by InventorySlotUI when an item is clicked, to assign it to the currently selected quick slot
    public void OnInventoryItemClicked(string itemName)
    {
        if (quickSlotSystem == null)
        {
            Debug.LogError("QuickSlotSystem missing.");
            return;
        }

        Debug.Log("ASSIGN ITEM -> " + itemName + " INTO SLOT " + (quickSlotSystem.SelectedSlotIndex + 1));
        quickSlotSystem.AssignToSelectedSlot(itemName);
    }

    // Refresh the inventory UI to show current items and quantities, and adjust visible slots based on unique item count
    public void RefreshInventoryUI()
    {
        if (inventorySystem == null || inventorySlots == null || inventorySlots.Length == 0)
            return;

        int uniqueCount = inventorySystem.inventory.Count;
        int visibleSlots = uniqueCount <= 6 ? 6 : Mathf.Clamp(uniqueCount, 6, 12);

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null)
                continue;

            bool shouldBeVisible = i < visibleSlots;
            inventorySlots[i].gameObject.SetActive(shouldBeVisible);

            if (!shouldBeVisible)
                continue;

            if (i < uniqueCount)
            {
                InventoryItem item = inventorySystem.inventory[i];
                inventorySlots[i].SetItem(item.itemName, item.quantity);
            }
            else
            {
                inventorySlots[i].SetEmpty();
            }
        }
    }
}