using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private int slotIndex;

    private QuickSlotSystem quickSlotSystem;
    private InventorySystem inventorySystem;
    private Button button;

    private void Awake()
    {
        quickSlotSystem = FindObjectOfType<QuickSlotSystem>();
        inventorySystem = FindObjectOfType<InventorySystem>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnEnable()
    {
        if (quickSlotSystem != null)
            quickSlotSystem.OnQuickSlotsChanged += Refresh;

        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged += Refresh;
    }

    private void OnDisable()
    {
        if (quickSlotSystem != null)
            quickSlotSystem.OnQuickSlotsChanged -= Refresh;

        if (inventorySystem != null)
            inventorySystem.OnInventoryChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnClicked()
    {
        if (quickSlotSystem == null)
            return;

        quickSlotSystem.SelectSlot(slotIndex);
        Debug.Log("Clicked quick slot " + (slotIndex + 1));
    }

    public void Refresh()
    {
        if (label == null || quickSlotSystem == null)
            return;

        string itemName = quickSlotSystem.GetItemInSlot(slotIndex);
        int quantity = quickSlotSystem.GetQuantityInSlot(slotIndex);

        if (string.IsNullOrEmpty(itemName) || quantity <= 0)
            label.text = "Slot " + (slotIndex + 1);
        else
            label.text = itemName + " x" + quantity;
    }
}