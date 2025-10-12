using UnityEngine;
using UnityEngine.InputSystem;

public class RingRotate : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 previousMousePos;
    private float previousZ = 0f;

    [SerializeField] private float rotationDirection = -1f;
    [SerializeField] private float snapThreshold = 5f;

    [Header("Audio")]
    public AudioSource grindingAudio;      // The looping grind sound
    [Range(0f, 1f)] public float maxVolume = 0.8f;
    [Range(0f, 10f)] public float volumeSensitivity = 2f;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogWarning("Tag your main camera!");

        previousZ = transform.localEulerAngles.z;

        if (grindingAudio != null)
        {
            grindingAudio.loop = true;
            grindingAudio.playOnAwake = false;
            grindingAudio.volume = 0f;
        }
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

                    if (grindingAudio != null && !grindingAudio.isPlaying)
                        grindingAudio.Play();
                }
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (isDragging)
            {
                isDragging = false;
                SnapIfWithinThreshold();

                if (grindingAudio != null)
                    grindingAudio.Stop();
            }
        }

        if (isDragging)
        {
            RotateRing(mousePos);
            previousMousePos = mousePos;
        }

        UpdateGrindingAudio();
    }

    private void RotateRing(Vector2 currentMousePos)
    {
        Vector3 centerScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 prevDir = ((Vector2)previousMousePos - (Vector2)centerScreenPos).normalized;
        Vector2 currDir = ((Vector2)currentMousePos - (Vector2)centerScreenPos).normalized;

        float angle = Vector2.SignedAngle(prevDir, currDir);
        float deltaRotation = angle * rotationDirection;

        transform.Rotate(0f, 0f, deltaRotation, Space.Self);
    }

    private void UpdateGrindingAudio()
    {
        if (grindingAudio == null || !grindingAudio.isPlaying) return;

        // Calculate how much the local Z changed since last frame
        float currentZ = transform.localEulerAngles.z;
        float deltaZ = Mathf.DeltaAngle(previousZ, currentZ);
        previousZ = currentZ;

        // Absolute rotation speed
        float angularSpeed = Mathf.Abs(deltaZ) / Time.deltaTime;

        // Scale volume with speed
        float targetVolume = Mathf.Clamp01((angularSpeed / 360f) * volumeSensitivity) * maxVolume;
        grindingAudio.volume = Mathf.MoveTowards(grindingAudio.volume, targetVolume, Time.deltaTime * 10f);
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
