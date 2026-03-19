using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 9f;
    public float gravity = -20f;
    public float jumpHeight = 1.2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Sync display-only speed stat at startup
        if (PlayerStats.Instance != null && PlayerStats.Instance.speed != moveSpeed)
        {
            PlayerStats.Instance.speed = moveSpeed;
            PlayerStats.Instance.SaveStats();
        }
    }

    private void Update()
    {
        // Keep speed stat synced with actual movement speed 
        if (PlayerStats.Instance != null && PlayerStats.Instance.speed != moveSpeed)
        {
            PlayerStats.Instance.speed = moveSpeed;
            PlayerStats.Instance.SaveStats();
        }

        // Ground check
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // Basic WASD movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}