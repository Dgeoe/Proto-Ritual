using UnityEngine;
using UnityEngine.InputSystem;

public class ClickyTester : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] private Camera mainCamera;

    [Header("Object References")]
    public GameObject TrackBars;
    public LiquidMixer LiquidMix;
    public BoxCollider[] BoxColliders;

    private Collider myCollider;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (BoxColliders != null)
        {
            foreach (BoxCollider box in BoxColliders)
                box.enabled = false;
        }
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
    }

    private void OnClicked()
    {
        Debug.Log($"{gameObject.name} was clicked!");

        if (TrackBars != null)
            TrackBars.SetActive(true);

        if (BoxColliders != null)
        {
            foreach (BoxCollider box in BoxColliders)
                box.enabled = true;
        }

        if (LiquidMix != null)
            LiquidMix.ClearAllFills();

        if (myCollider != null)
            myCollider.enabled = false;

        Debug.Log("ClickyTester interaction triggered.");
    }
}
