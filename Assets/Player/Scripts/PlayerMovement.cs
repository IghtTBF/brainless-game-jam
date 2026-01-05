using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private CharacterController player;

    private InputAction move;
    private InputAction look;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookSpeed;
    private float gravity = -9.81f;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private Vector3 Velocity;
    private Vector3 camOffset = new Vector3(0, 0.5f, 0);
    private float rotationX;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        move = playerInputActions.Player.Move;
        move.Enable();

        look = playerInputActions.Player.Look;
        look.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        look.Disable();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = GetComponent<CharacterController>();
    }
    void Update()
    {
        moveInput = move.ReadValue<Vector2>();

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        if (player.isGrounded && Velocity.y > 0f)
        {
            Velocity.y = -2f;
        }

        Vector3 moveDirection = (right * moveInput.x + forward * moveInput.y).normalized;
        Velocity = new Vector3(moveDirection.x * moveSpeed, Velocity.y, moveDirection.z * moveSpeed);
        Velocity.y += gravity;

        player.Move(Velocity * Time.deltaTime);
    }

    void LateUpdate()
    {
        lookInput = look.ReadValue<Vector2>();

        rotationX -= lookInput.y * lookSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -65, 45);

        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);

        playerCam.transform.rotation = transform.rotation * Quaternion.Euler(rotationX,0,0);
        playerCam.transform.position = transform.position + camOffset;
    }
}
