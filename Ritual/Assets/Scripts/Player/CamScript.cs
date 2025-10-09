using UnityEngine;
using UnityEngine.InputSystem;

public class CamScript : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensX = 100f;
    public float sensY = 100f;

    [Header("References")]
    public Transform orientation; 

    private PlayerControls controls;
    private Vector2 lookInput;

    private float xRotation;
    private float yRotation; 

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = lookInput.x * sensX * Time.deltaTime;
        float mouseY = lookInput.y * sensY * Time.deltaTime;

        // Adjust rotations
        yRotation += mouseX;        
        xRotation -= mouseY;        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0); 
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);     
    }
}
