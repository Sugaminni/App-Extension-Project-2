using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private Button button;

    private string itemName;
    private InventoryUIController inventoryUIController;

    private void Awake()
    {
        inventoryUIController = FindObjectOfType<InventoryUIController>();

        if (button == null)
            button = GetComponent<Button>();

        if (itemText == null)
            itemText = GetComponentInChildren<TMP_Text>();

        if (button != null)
        {
            button.onClick.RemoveListener(ClickSlot);
            button.onClick.AddListener(ClickSlot);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no Button component.");
        }
    }

    public void SetItem(string newItemName, int quantity)
    {
        itemName = newItemName;

        if (itemText != null)
            itemText.text = newItemName + " x" + quantity;

        if (button != null)
            button.interactable = true;
    }

    public void SetEmpty()
    {
        itemName = "";

        if (itemText != null)
            itemText.text = "Empty";

        if (button != null)
            button.interactable = false;
    }

    private void ClickSlot()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("Clicked empty slot.");
            return;
        }

        if (inventoryUIController == null)
            inventoryUIController = FindObjectOfType<InventoryUIController>();

        if (inventoryUIController != null)
        {
            Debug.Log("Clicked inventory slot: " + itemName);
            inventoryUIController.OnInventoryItemClicked(itemName);
        }
        else
        {
            Debug.LogError("InventoryUIController not found.");
        }
    }
}