using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    [Header("Input")]
    public InputActionAsset actionsAsset;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction lookAction;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 velocity;
    private float xRotation = 0f;

    
    public System.Action OnPlayerMoved;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        moveAction = actionsAsset.FindAction("Player/Move", true);
        lookAction = actionsAsset.FindAction("Player/Look", true);

        moveAction.Enable();
        lookAction.Enable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        HandleLook();
        HandleMovement();
    }

    void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
    }


    public void ResetLookInput()
    {
        lookInput = Vector2.zero;
    }


    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = move + velocity;
        controller.Move(finalMove * Time.deltaTime);

        
        if (moveInput.magnitude > 0.1f && OnPlayerMoved != null)
            OnPlayerMoved.Invoke();
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }
}
