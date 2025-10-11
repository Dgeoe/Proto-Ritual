using UnityEngine;
using UnityEngine.InputSystem;

public class RingRotate : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 previousMousePos;

    [SerializeField] private float rotationDirection = -1f;
    [SerializeField] private float snapThreshold = 5f;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogWarning("You gotta tag your camera as main camera idiot");
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    previousMousePos = mousePos;
                }
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (isDragging)
            {
                isDragging = false;
                SnapIfWithinThreshold();
            }
        }

        if (isDragging)
        {
            RotateRing(mousePos);
            previousMousePos = mousePos;
        }
    }

    private void RotateRing(Vector2 currentMousePos)
    {
        Vector3 centerScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 prevDir = ((Vector2)previousMousePos - (Vector2)centerScreenPos).normalized;
        Vector2 currDir = ((Vector2)currentMousePos - (Vector2)centerScreenPos).normalized;

        float angle = Vector2.SignedAngle(prevDir, currDir);

        transform.Rotate(0f, 0f, angle * rotationDirection, Space.Self);
    }

    private void SnapIfWithinThreshold()
    {
        Vector3 localEuler = transform.localEulerAngles;
        float currentZ = localEuler.z;

        float nearest90 = Mathf.Round(currentZ / 90f) * 90f;
        float delta = Mathf.DeltaAngle(currentZ, nearest90);

        if (Mathf.Abs(delta) <= snapThreshold)
        {
            localEuler.z = nearest90;
            transform.localEulerAngles = localEuler;
        }
    }

    
    public void ResetRotation()
    {
        Vector3 localEuler = transform.localEulerAngles;
        localEuler.z = -90f;  
        transform.localEulerAngles = localEuler;
    }

}
