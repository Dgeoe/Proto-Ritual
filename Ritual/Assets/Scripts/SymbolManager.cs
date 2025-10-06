using UnityEngine;

public class SymbolManager : MonoBehaviour
{
    [Header("Five Symbols")]
    public GameObject triangleSymbol;
    public GameObject starSymbol;
    public GameObject squareSymbol;
    public GameObject eyeballSymbol;
    public GameObject doubleEyeballSymbol;

    [Header("For reference")]
    public SymbolType activeSymbolType;
    private GameObject activeSymbolObject;

    public enum SymbolType
    {
        Triangle,
        Star,
        Square,
        Eyeball,
        DoubleEyeball
    }

    private void Awake()
    {
        // Deactivate all symbols first
        GameObject[] symbols = { triangleSymbol, starSymbol, squareSymbol, eyeballSymbol, doubleEyeballSymbol };
        foreach (GameObject symbol in symbols)
        {
            if (symbol != null)
                symbol.SetActive(false);
        }

        // Randomly select active symbol
        int index = Random.Range(0, symbols.Length);
        activeSymbolObject = symbols[index];
        activeSymbolObject.SetActive(true);

        activeSymbolType = (SymbolType)index;

        Debug.Log("Active Symbol (Awake): " + activeSymbolType);
    }

    public SymbolType GetActiveSymbol()
    {
        return activeSymbolType;
    }

    public GameObject GetActiveSymbolObject()
    {
        return activeSymbolObject;
    }
}
