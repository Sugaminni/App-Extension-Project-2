using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] inventorySlots;

    [Header("Input")]
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;
    [SerializeField] private float toggleCooldown = 0.25f;

    private InventorySystem inventorySystem;
    [SerializeField] private QuickSlotSystem quickSlotSystem;

    private string selectedItemName;
    private float nextToggleTime = 0f;

    private void Awake()
    {
        // Prevent duplicate InventoryUIControllers from both toggling.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate InventoryUIController found and disabled on: " + gameObject.name);
            enabled = false;
            return;
        }

        Instance = this;

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

        RefreshInventoryUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged -= RefreshInventoryUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey) && Time.time >= nextToggleTime)
        {
            nextToggleTime = Time.time + toggleCooldown;

            Debug.Log("Inventory key pressed by: " + gameObject.name);
            ToggleInventory();
        }
    }

    public bool IsOpen()
    {
        return inventoryPanel != null && inventoryPanel.activeSelf;
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogWarning("Inventory panel is not assigned.");
            return;
        }

        bool nextState = !inventoryPanel.activeSelf;

        inventoryPanel.SetActive(nextState);

        if (nextState)
        {
            CursorManager.SetUIMode();
            RefreshInventoryUI();
            Debug.Log("Inventory opened.");
        }
        else
        {
            CursorManager.SetGameplayMode();
            Debug.Log("Inventory closed.");
        }
    }

    public void OpenInventory()
    {
        if (inventoryPanel == null)
            return;

        if (inventoryPanel.activeSelf)
            return;

        inventoryPanel.SetActive(true);
        CursorManager.SetUIMode();
        RefreshInventoryUI();

        Debug.Log("Inventory opened.");
    }

    public void CloseInventory()
    {
        if (inventoryPanel == null)
            return;

        if (!inventoryPanel.activeSelf)
            return;

        inventoryPanel.SetActive(false);
        CursorManager.SetGameplayMode();

        Debug.Log("Inventory closed.");
    }

    public void OnInventoryItemClicked(string itemName)
    {
        selectedItemName = itemName;
        Debug.Log("Selected item: " + selectedItemName);
    }

    public void AssignSelectedToSlot1()
    {
        AssignSelectedToSlot(0);
    }

    public void AssignSelectedToSlot2()
    {
        AssignSelectedToSlot(1);
    }

    public void AssignSelectedToSlot3()
    {
        AssignSelectedToSlot(2);
    }

    public void AssignSelectedToSlot4()
    {
        AssignSelectedToSlot(3);
    }

    private void AssignSelectedToSlot(int slotIndex)
    {
        Debug.Log("Trying to assign item: " + selectedItemName + " to slot " + (slotIndex + 1));

        if (quickSlotSystem == null)
        {
            Debug.LogError("QuickSlotSystem missing.");
            return;
        }

        if (string.IsNullOrEmpty(selectedItemName))
        {
            Debug.LogWarning("No inventory item selected.");
            return;
        }

        quickSlotSystem.AssignToSlot(slotIndex, selectedItemName);

        Debug.Log("Assign call completed.");
    }

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