using UnityEngine;
using UnityEngine.InputSystem;

public class TypeFocus : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetYRotation = 90f;

    private bool isInTypeMode = false;
    private bool isTransitioning = false;
    private float transitionProgress = 0f;

    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private Collider myCollider;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        myCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isInTypeMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == myCollider)
                    EnterTypeMode();
            }
        }

        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime * moveSpeed;
            float t = Mathf.SmoothStep(0f, 1f, transitionProgress);

            playerTransform.position = Vector3.Lerp(startPos, targetPos, t);
            playerTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            if (transitionProgress >= 1f)
                isTransitioning = false;
        }

        if (isInTypeMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed ||
                Keyboard.current.dKey.isPressed)
            {
                ExitTypeMode();
            }
        }
    }

    public void EnterTypeMode()
    {
        if (playerTransform == null || focusPosition == null || playerMovement == null) return;

        isInTypeMode = true;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;

        targetPos = focusPosition.position;
        Vector3 euler = playerTransform.eulerAngles;
        euler.y = targetYRotation;
        targetRot = Quaternion.Euler(euler);

        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (myCollider != null)
            myCollider.enabled = false;
    }

    public void ExitTypeMode()
    {
        if (!isInTypeMode) return;

        isInTypeMode = false;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;
        targetPos = startPos; 
        targetRot = startRot;

        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (myCollider != null)
            myCollider.enabled = true;
    }
}
