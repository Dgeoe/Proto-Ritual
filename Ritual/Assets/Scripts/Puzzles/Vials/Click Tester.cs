using UnityEngine;
using UnityEngine.InputSystem;

public class ClickTester : MonoBehaviour
{
    [Header("Camera and Player References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private PlayerMovement playerMovement;
    public GameObject TrackBars;
    public LiquidMixer LiquidMix;
    public BoxCollider[] BoxColliders;

    [Header("Transition Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private bool isInFocusMode = false;
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

        foreach (BoxCollider box in BoxColliders)
        {
            box.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerMovement != null)
            playerMovement.OnPlayerMoved += ExitFocusMode;
    }

    private void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.OnPlayerMoved -= ExitFocusMode;
    }

    void Update()
    {
        // Detect click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                    OnClicked();
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

        if (isInFocusMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed ||
                Keyboard.current.dKey.isPressed)
            {
                ExitFocusMode();
            }
        }

        if (transitionProgress >= 1f)
        {
            isTransitioning = false;

            if (!isInFocusMode)
                playerMovement.enabled = true; 
        }

    }

    private void OnClicked()
    {
        Debug.Log($"{gameObject.name} was clicked!");

        if (!isInFocusMode)
        {
            EnterFocusMode();
        }
    }

    private void EnterFocusMode()
    {

        if (playerMovement == null || focusPosition == null || playerTransform == null) return;

        isInFocusMode = true;
        isTransitioning = true;
        transitionProgress = 0f;

        originalPos = playerTransform.position;
        originalRot = playerTransform.rotation;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;
        targetPos = focusPosition.position;
        targetRot = focusPosition.rotation;

        playerMovement.enabled = false;
        if (myCollider != null)
            myCollider.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (TrackBars != null)
        {
            TrackBars.SetActive(true);
        }
        else
        {
            Debug.Log("Typewriter Enter");
        }

        foreach (BoxCollider box in BoxColliders)
        {
            box.enabled = true;
        }

    }

    private void ExitFocusMode()
    {
        if (!isInFocusMode) return;

        isInFocusMode = false;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;
        targetPos = originalPos;
        targetRot = originalRot;

        playerMovement.enabled = false; 
        if (myCollider != null)
            myCollider.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (TrackBars != null)
            TrackBars.SetActive(false);

        if (LiquidMix != null)
            LiquidMix.ClearAllFills();

        Debug.Log("Returning from focus mode");

        foreach (BoxCollider box in BoxColliders)
        {
            box.enabled = false;
        }
    }

}
