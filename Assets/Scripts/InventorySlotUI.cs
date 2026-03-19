using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public TMP_Text label;
    public Button button;

    private string currentItemName;
    private bool isEmpty = true;
    private InventoryUIController inventoryUI;

    private void Awake()
    {
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUIController>();

        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSlotClicked);
        }
    }

    // Initialize the inventory slot with a reference to the InventoryUIController, and set up the button click listener
    public void Initialize(InventoryUIController controller)
    {
        inventoryUI = controller;

        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSlotClicked);
        }
    }

    // Set the inventory slot to an empty state, clearing any item information and making it non-interactable
    public void SetEmpty()
    {
        isEmpty = true;
        currentItemName = null;

        if (label != null)
            label.text = "Empty";

        if (button != null)
            button.interactable = false;
    }

    // Set the inventory slot to display a specific item and quantity, and make it interactable
    public void SetItem(string itemName, int qty)
    {
        isEmpty = false;
        currentItemName = itemName;

        if (label != null)
            label.text = $"{itemName} x{qty}";

        if (button != null)
            button.interactable = true;
    }

    // Called when the inventory slot button is clicked. If the slot is not empty, it will notify the InventoryUIController to assign this item to the currently selected quick slot.
    private void OnSlotClicked()
    {
        if (isEmpty || string.IsNullOrEmpty(currentItemName))
            return;

        if (inventoryUI == null)
        {
            Debug.LogError("InventorySlotUI: inventoryUI is null on " + gameObject.name);
            return;
        }

        Debug.Log("Clicked inventory item -> " + currentItemName);
        inventoryUI.OnInventoryItemClicked(currentItemName);
    }
}