using UnityEngine;
using UnityEngine.InputSystem;

public class RingRitual : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Transform ringFocusPosition;

    [Header("Transition Settings")]
    public float moveSpeedToRing = 2f;
    public float rotationSpeedToRing = 2f;

    private bool isInRingMode = false;
    private bool isTransitioning = false;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private float transitionProgress = 0f;

    public bool IsInRingMode => isInRingMode; 
    
    //Don't ask me why there's 4 different scripts for this one thing
    //I'm sick and for some reason this is the only way it doesn't break
    //I'll try fix it lateerrrrrrrr

    void Update()
    {
        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime * moveSpeedToRing;
            float t = Mathf.SmoothStep(0f, 1f, transitionProgress);

            playerMovement.transform.position = Vector3.Lerp(startPos, targetPos, t);
            playerMovement.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            if (transitionProgress >= 1f)
                isTransitioning = false;
        }

        if (isInRingMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                ExitRingMode();
            }
        }
    }

    public void EnterRingMode()
    {
        if (playerMovement == null || ringFocusPosition == null) return;

        isInRingMode = true;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerMovement.transform.position;
        startRot = playerMovement.transform.rotation;

        targetPos = ringFocusPosition.position;

        float currentX = playerMovement.transform.rotation.eulerAngles.x;
        float currentZ = playerMovement.transform.rotation.eulerAngles.z;
        targetRot = Quaternion.Euler(currentX, 312f, currentZ);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;
    }

    public void ExitRingMode()
    {
        if (!isInRingMode) return;

        isInRingMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.enabled = true;

        playerMovement.ResetLookInput();
    }

    
}
