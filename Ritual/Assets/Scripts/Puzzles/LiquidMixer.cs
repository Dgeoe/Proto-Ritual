using UnityEngine;

public class LiquidMixer : MonoBehaviour
{
    public enum LiquidType { Red, Green, Yellow, Blue, Mix }

    [Header("Liquid Mixer Settings")]
    public LiquidType currentSelection = LiquidType.Mix;

    [Header("References")]
    public LiquidHolder liquidHolder;

    [Header("Active Color States (Debug)")]
    public bool redActive;
    public bool greenActive;
    public bool yellowActive;
    public bool blueActive;

    [Header("Fill Levels")]
    [Range(0, 100)] public float redFill;
    [Range(0, 100)] public float greenFill;
    [Range(0, 100)] public float yellowFill;
    [Range(0, 100)] public float blueFill;

    private void OnMouseDown()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (currentSelection != LiquidType.Mix)
        {
            // Selecting a color
            SetActiveColor(currentSelection);
        }
        else
        {
            // Mix object clicked
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

    private void SetActiveColor(LiquidType type)
    {
        redActive = (type == LiquidType.Red);
        greenActive = (type == LiquidType.Green);
        yellowActive = (type == LiquidType.Yellow);
        blueActive = (type == LiquidType.Blue);

        Debug.Log($"Selected color: {type}");
    }

    private void AddFill()
    {
        if (redActive)
        {
            redFill = Mathf.Min(100, redFill + 25);
            Debug.Log($"Red fill: {redFill}");
        }
        else if (greenActive)
        {
            greenFill = Mathf.Min(100, greenFill + 25);
            Debug.Log($"Green fill: {greenFill}");
        }
        else if (yellowActive)
        {
            yellowFill = Mathf.Min(100, yellowFill + 25);
            Debug.Log($"Yellow fill: {yellowFill}");
        }
        else if (blueActive)
        {
            blueFill = Mathf.Min(100, blueFill + 25);
            Debug.Log($"Blue fill: {blueFill}");
        }
        else
        {
            Debug.LogWarning("No color selected before mixing!");
        }
    }

    private void ClearAllFills()
    {
        redFill = greenFill = yellowFill = blueFill = 0f;
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

