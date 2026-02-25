using TMPro;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    public TMP_Text label;

    public void SetEmpty()
    {
        label.text = "Empty";
    }

    public void SetItem(string itemName, int qty)
    {
        label.text = $"{itemName} x{qty}";
    }
}