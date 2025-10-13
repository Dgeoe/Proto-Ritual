using UnityEngine;
using UnityEngine.InputSystem;

public class AlterFocus : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetYRotation = 90f;
    [SerializeField] private BoxCollider[] boxColliders = new BoxCollider[4];

    private bool isInAltarMode = false;
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
        if (!isInAltarMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == myCollider)
                    EnterAltarMode();
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

        if (isInAltarMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed ||
                Keyboard.current.dKey.isPressed)
            {
                ExitAltarMode();
            }
        }
    }

    public void EnterAltarMode()
    {
        if (playerTransform == null || focusPosition == null || playerMovement == null) return;

        isInAltarMode = true;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;

        targetPos = focusPosition.position;
        targetRot = Quaternion.Euler(73f, targetYRotation, playerTransform.eulerAngles.z);

        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (BoxCollider box in boxColliders)
        {
            if (box != null)
                box.enabled = true;
        }

        if (myCollider != null)
            myCollider.enabled = false;
    }

    public void ExitAltarMode()
    {
        if (!isInAltarMode) return;

        isInAltarMode = false;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;

        targetPos = startPos;
        targetRot = Quaternion.Euler(0f, 0f, 0f);

        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (BoxCollider box in boxColliders)
        {
            if (box != null)
                box.enabled = false;
        }

        if (myCollider != null)
            myCollider.enabled = true;
    }
}
