using UnityEngine;
using UnityEngine.InputSystem;

public class LiquidMixer : MonoBehaviour
{
    public enum LiquidType { Red, Green, Yellow, Blue, Mix }

    [Header("Liquid Mixer Settings")]
    public LiquidType currentSelection = LiquidType.Mix;

    [Header("References")]
    public LiquidHolder liquidHolder;

    [Header("Fill Levels")]
    [Range(0, 100)] public float redFill;
    [Range(0, 100)] public float greenFill;
    [Range(0, 100)] public float yellowFill;
    [Range(0, 100)] public float blueFill;

    [SerializeField] private Camera mainCamera;

    // Instance events (display subscribes to this specific mixer instance)
    public event System.Action<LiquidType> OnColorAdded;
    public event System.Action OnMixCleared;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // for reset 
                bool shiftHeld =
                    Keyboard.current.leftShiftKey.isPressed ||
                    Keyboard.current.rightShiftKey.isPressed;

                if (currentSelection != LiquidType.Mix)
                {
                    // Selecting a color
                    SetActiveColor(currentSelection);
                }
                else
                {
                    if (shiftHeld)
                    {
                        ClearAllFills();
                        Debug.Log("All fills cleared!");
                    }
                    else
                    {
                        AddFill();
                        CheckForSuccess();
                    }
                }
            }
        }
    }

    private void SetActiveColor(LiquidType type)
    {
        // keep track of the selection too
        currentSelection = type;

        liquidHolder.redActive = (type == LiquidType.Red);
        liquidHolder.greenActive = (type == LiquidType.Green);
        liquidHolder.yellowActive = (type == LiquidType.Yellow);
        liquidHolder.blueActive = (type == LiquidType.Blue);

        Debug.Log($"Selected color: {type}");
    }

    private void AddFill()
    {
        if (liquidHolder == null)
        {
            Debug.LogError("No LiquidHolder assigned!");
            return;
        }

        LiquidType? added = null;

        if (liquidHolder.redActive)
        {
            redFill = Mathf.Min(100, redFill + 25);
            added = LiquidType.Red;
            Debug.Log($"Red fill: {redFill}");
        }
        else if (liquidHolder.greenActive)
        {
            greenFill = Mathf.Min(100, greenFill + 25);
            added = LiquidType.Green;
            Debug.Log($"Green fill: {greenFill}");
        }
        else if (liquidHolder.yellowActive)
        {
            yellowFill = Mathf.Min(100, yellowFill + 25);
            added = LiquidType.Yellow;
            Debug.Log($"Yellow fill: {yellowFill}");
        }
        else if (liquidHolder.blueActive)
        {
            blueFill = Mathf.Min(100, blueFill + 25);
            added = LiquidType.Blue;
            Debug.Log($"Blue fill: {blueFill}");
        }
        else
        {
            Debug.LogWarning("No color selected before mixing!");
        }

        if (added.HasValue)
        {
            Debug.Log($"LiquidMixer: invoking OnColorAdded -> {added.Value}");
            OnColorAdded?.Invoke(added.Value);
        }
    }

    private void ClearAllFills()
    {
        redFill = greenFill = yellowFill = blueFill = 0f;
        Debug.Log("LiquidMixer: cleared fills -> invoking OnMixCleared");
        OnMixCleared?.Invoke();
    }

    private void CheckForSuccess()
    {
        if (liquidHolder == null)
        {
            Debug.LogError("No LiquidHolder assigned!");
            return;
        }

        bool success = liquidHolder.CheckRatios(redFill, greenFill, yellowFill, blueFill);

        if (success)
            Debug.Log("YAY!!!! PotioTime");
        else
            Debug.Log("Keep mixing!");
    }
}
