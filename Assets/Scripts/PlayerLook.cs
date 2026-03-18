using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraPivot;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical (look up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal (turn player)
        transform.Rotate(Vector3.up * mouseX);
    }
}