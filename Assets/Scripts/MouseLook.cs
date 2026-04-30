using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraPivot;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private InventoryUIController inventoryUI;

    private void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUIController>();
    }

    private void Update()
    {
        if (CursorManager.IsUIOpen)
            return;

        if (inventoryUI != null && inventoryUI.IsOpen())
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}