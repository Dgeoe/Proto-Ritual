using UnityEngine;
using UnityEngine.InputSystem;

public class VialsFocus : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float targetYRotation = 90f;
    [SerializeField] private BoxCollider[] boxColliders = new BoxCollider[5];

    [Header("Liquid Mixer Reference")]
    public LiquidMixer liquidMixer;

    private bool isInVialMode = false;
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
        if (!isInVialMode && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == myCollider)
                    EnterVialMode();
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

        if (isInVialMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed ||
                Keyboard.current.dKey.isPressed)
            {
                ExitVialMode();
            }
        }
    }

    public void EnterVialMode()
    {
        if (playerTransform == null || focusPosition == null || playerMovement == null) return;

        isInVialMode = true;
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

        foreach (BoxCollider box in boxColliders)
        {
            if (box != null)
                box.enabled = true;
        }

        if (myCollider != null)
            myCollider.enabled = false;
    }

    public void ExitVialMode()
    {
        if (!isInVialMode) return;

        isInVialMode = false;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerTransform.position;
        startRot = playerTransform.rotation;
        targetPos = focusPosition.position;
        targetRot = Quaternion.Euler(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);

        foreach (BoxCollider box in boxColliders)
        {
            if (box != null)
                box.enabled = false;
        }

        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (myCollider != null)
            myCollider.enabled = true;

        // Reset the LiquidMixer state
        if (liquidMixer != null)
        {
            liquidMixer.ClearAllFills();

            if (liquidMixer.HourglassLight != null)
                liquidMixer.HourglassLight.SetActive(false);

            if (liquidMixer.SuccessAudio != null)
                liquidMixer.SuccessAudio.Stop();

            Debug.Log($"Fills after reset: R:{liquidMixer.redFill}, G:{liquidMixer.greenFill}, Y:{liquidMixer.yellowFill}, B:{liquidMixer.blueFill}");
        }
        else
        {
            Debug.LogWarning("LiquidMixer reference not set in VialsFocus!");
        }
    }
}
