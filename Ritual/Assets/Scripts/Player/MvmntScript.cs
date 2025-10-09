using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MvmntScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float maxSpeed = 8f;
    public float airMultiplier = 0.5f;
    public float dragGround = 5f;
    public float dragAir = 0.5f;

    [Header("References")]
    public Transform orientation; 

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    private bool grounded;

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        rb.linearDamping = grounded ? dragGround : dragAir;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 moveDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        moveDir.Normalize();

        // Apply movement force
        if (grounded)
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}

