using UnityEngine;

[RequireComponent(typeof(SymbolManager))]
public class LiquidHolder : MonoBehaviour
{
    [Header("Color Ratio Targets")]
    public float redTarget;
    public float greenTarget;
    public float yellowTarget;
    public float blueTarget;
    public SymbolManager.SymbolType activeSymbol;
    public bool redActive;
    public bool greenActive;
    public bool yellowActive;
    public bool blueActive;
    public SymbolManager symbolManager;
    private void Start()
    {
        SetValuesBasedOnSymbol();
    }

    private void SetValuesBasedOnSymbol()
    {
        // Reset all first
        redTarget = greenTarget = yellowTarget = blueTarget = 0f;

        activeSymbol = symbolManager.GetActiveSymbol();

        switch (activeSymbol)
        {
            case SymbolManager.SymbolType.Triangle:
                // Red + Blue = 75 / 25
                redTarget = 75;
                blueTarget = 25;
                break;

            case SymbolManager.SymbolType.Star:
                // Yellow + Red = 50 / 50
                yellowTarget = 50;
                redTarget = 50;
                break;

            case SymbolManager.SymbolType.Square:
                // Green + Blue = 50 / 50
                greenTarget = 50;
                blueTarget = 50;
                break;

            case SymbolManager.SymbolType.Eyeball:
                // Red + Green = 75 / 25
                redTarget = 75;
                greenTarget = 25;
                break;

            case SymbolManager.SymbolType.DoubleEyeball:
                // Yellow + Blue = 75 / 25
                yellowTarget = 75;
                blueTarget = 25;
                break;
        }

        Debug.Log($"[LiquidHolder] Active Symbol: {activeSymbol} | Target Ratios -> R:{redTarget}, G:{greenTarget}, Y:{yellowTarget}, B:{blueTarget}");
    }

    public bool CheckRatios(float redFill, float greenFill, float yellowFill, float blueFill)
    {
        bool match = Mathf.Approximately(redFill, redTarget)
                     && Mathf.Approximately(greenFill, greenTarget)
                     && Mathf.Approximately(yellowFill, yellowTarget)
                     && Mathf.Approximately(blueFill, blueTarget);

        return match;
    }
}

