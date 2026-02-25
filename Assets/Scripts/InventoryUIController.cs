using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Controls")]
    [SerializeField] private KeyCode toggleKey = KeyCode.I;

    private bool isOpen;

    private void Start()
    {
        // Start hidden
        SetOpen(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetOpen(!isOpen);
        }
    }

    private void SetOpen(bool open)
    {
        isOpen = open;
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);
    }
}