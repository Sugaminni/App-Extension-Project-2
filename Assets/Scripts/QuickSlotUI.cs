using UnityEngine;
using TMPro;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private QuickSlotSystem quickSlotSystem;
    [SerializeField] private TMP_Text[] slotTexts;

    private void Awake()
    {
        if (quickSlotSystem == null)
            quickSlotSystem = FindObjectOfType<QuickSlotSystem>();
    }

    private void OnEnable()
    {
        if (quickSlotSystem != null)
            quickSlotSystem.OnQuickSlotsChanged += RefreshUI;

        RefreshUI();
    }

    private void OnDisable()
    {
        if (quickSlotSystem != null)
            quickSlotSystem.OnQuickSlotsChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        if (quickSlotSystem == null || slotTexts == null)
            return;

        for (int i = 0; i < slotTexts.Length; i++)
        {
            if (slotTexts[i] == null)
                continue;

            string itemName = quickSlotSystem.GetItemInSlot(i);
            int quantity = quickSlotSystem.GetQuantityInSlot(i);

            if (string.IsNullOrEmpty(itemName))
            {
                slotTexts[i].text = "Slot " + (i + 1) + "\nEmpty";
            }
            else
            {
                slotTexts[i].text = "Slot " + (i + 1) + "\n" + itemName + " x" + quantity;
            }
        }
    }
}