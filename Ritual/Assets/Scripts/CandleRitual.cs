using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CandleRitual : MonoBehaviour
{
    [Header("Assign your candle GameObjects")]
    public GameObject candle1;
    public GameObject candle2;
    public GameObject candle3;
    public GameObject candle4;
    public GameObject candle5;
    public GameObject candle6;

    [Header("Candle Flames")]
    public ParticleSystem flame1;
    public ParticleSystem flame2;
    public ParticleSystem flame3;
    public ParticleSystem flame4;
    public ParticleSystem flame5;
    public ParticleSystem flame6;

    [Header("Ritual Settings")]
    public SymbolManager symbolManager;
    [HideInInspector] public int numberOfActiveCandles;

    [Header("Player Interaction")]
    public PlayerMovement playerMovement;
    public Transform candleFocusPosition;
    private bool isInCandleMode = false;

    [Header("Candle Mode Transition")]
    public float moveSpeedToCandle = 2f;
    public float rotationSpeedToCandle = 2f;

    [Header("Audio")]
    public AudioSource ritualCompleteSound;
    public AudioSource candleClickSound;
    public AudioClip candleToggleClip;

    [Header("Lighting Effects")]
    public GameObject hourglassLight2;   

    private bool isTransitioning = false;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private float transitionProgress = 0f;

    private List<GameObject> activeCandles = new List<GameObject>();
    private List<ParticleSystem> candleFlames = new List<ParticleSystem>();
    private List<int> targetCandleIndexes = new List<int>();
    private List<int> playerLitCandles = new List<int>();

    private bool ritualCompleted = false;

    void OnEnable()
    {
        if (playerMovement != null)
            playerMovement.OnPlayerMoved += ExitCandleMode;
    }

    void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.OnPlayerMoved -= ExitCandleMode;
    }

    void Start()
    {
        numberOfActiveCandles = Random.Range(4, 7);
        Debug.Log("Number of candles this round: " + numberOfActiveCandles);

        SetupCandles();
        SetupFlames();
        DetermineTargetCandles();

        if (hourglassLight2 != null)
            hourglassLight2.SetActive(false); 
    }

    void Update()
    {
        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime * moveSpeedToCandle;
            float t = Mathf.SmoothStep(0f, 1f, transitionProgress);
            playerMovement.transform.position = Vector3.Lerp(startPos, targetPos, t);
            playerMovement.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            if (transitionProgress >= 1f)
                isTransitioning = false;
        }

        if (isInCandleMode && !isTransitioning)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
                Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                ExitCandleMode();
            }
        }
    }

    void SetupCandles()
    {
        candle1.SetActive(true);
        candle2.SetActive(true);
        candle3.SetActive(true);
        candle4.SetActive(true);
        activeCandles = new List<GameObject> { candle1, candle2, candle3, candle4 };

        if (numberOfActiveCandles >= 5)
        {
            candle5.SetActive(true);
            activeCandles.Add(candle5);
        }
        else candle5.SetActive(false);

        if (numberOfActiveCandles == 6)
        {
            candle6.SetActive(true);
            activeCandles.Add(candle6);
        }
        else candle6.SetActive(false);
    }

    void SetupFlames()
    {
        candleFlames = new List<ParticleSystem> { flame1, flame2, flame3, flame4 };

        if (numberOfActiveCandles >= 5)
            candleFlames.Add(flame5);
        if (numberOfActiveCandles == 6)
            candleFlames.Add(flame6);

        foreach (ParticleSystem flame in candleFlames)
        {
            if (flame != null)
                flame.Stop();
        }
    }

    void DetermineTargetCandles()
    {
        SymbolManager.SymbolType activeSymbol = symbolManager.GetActiveSymbol();
        Debug.Log("The Ritual Script believes the active symbol is: " + activeSymbol);

        targetCandleIndexes.Clear();

        switch (activeSymbol)
        {
            case SymbolManager.SymbolType.Triangle:
                if (numberOfActiveCandles == 4) targetCandleIndexes.AddRange(new int[] { 0, 1 });
                else if (numberOfActiveCandles == 5) targetCandleIndexes.AddRange(new int[] { 0, 2 });
                else targetCandleIndexes.AddRange(new int[] { 0, 3 });
                break;
            case SymbolManager.SymbolType.Star:
                if (numberOfActiveCandles == 4) targetCandleIndexes.AddRange(new int[] { 1, 2 });
                else if (numberOfActiveCandles == 5) targetCandleIndexes.AddRange(new int[] { 0, 4 });
                else targetCandleIndexes.AddRange(new int[] { 1, 5 });
                break;
            case SymbolManager.SymbolType.Square:
                if (numberOfActiveCandles == 4) targetCandleIndexes.AddRange(new int[] { 2, 3 });
                else if (numberOfActiveCandles == 5) targetCandleIndexes.AddRange(new int[] { 1, 3 });
                else targetCandleIndexes.AddRange(new int[] { 2, 4 });
                break;
            case SymbolManager.SymbolType.Eyeball:
                if (numberOfActiveCandles == 4) targetCandleIndexes.AddRange(new int[] { 0, 3 });
                else if (numberOfActiveCandles == 5) targetCandleIndexes.AddRange(new int[] { 1, 4 });
                else targetCandleIndexes.AddRange(new int[] { 0, 5 });
                break;
            case SymbolManager.SymbolType.DoubleEyeball:
                if (numberOfActiveCandles == 4) targetCandleIndexes.AddRange(new int[] { 0, 2 });
                else if (numberOfActiveCandles == 5) targetCandleIndexes.AddRange(new int[] { 1, 3 });
                else targetCandleIndexes.AddRange(new int[] { 2, 5 });
                break;
        }
    }

    public void ToggleCandle(int candleID)
    {
        Debug.Log("Candle " + (candleID + 1) + " clicked");

        if (!isInCandleMode)
        {
            EnterCandleMode();
            return;
        }

        bool wasLit = playerLitCandles.Contains(candleID);

        if (wasLit)
        {
            playerLitCandles.Remove(candleID);
            if (candleFlames[candleID] != null)
                candleFlames[candleID].Stop();
        }
        else
        {
            playerLitCandles.Add(candleID);
            if (candleFlames[candleID] != null)
                candleFlames[candleID].Play();
        }

        if (candleClickSound != null)
        {
            if (candleToggleClip != null)
                candleClickSound.PlayOneShot(candleToggleClip);
            else
                candleClickSound.Play();
        }

        CheckSuccess();
    }

    void EnterCandleMode()
    {
        if (playerMovement == null || candleFocusPosition == null) return;

        isInCandleMode = true;
        isTransitioning = true;
        transitionProgress = 0f;

        startPos = playerMovement.transform.position;
        startRot = playerMovement.transform.rotation;

        targetPos = candleFocusPosition.position;
        targetRot = Quaternion.Euler(playerMovement.transform.rotation.eulerAngles.x, -141f, playerMovement.transform.rotation.eulerAngles.z);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;
    }

    void ExitCandleMode()
    {
        if (!isInCandleMode) return;

        isInCandleMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.enabled = true;

        playerMovement.ResetLookInput();
    }

    void CheckSuccess()
    {
        if (playerLitCandles.Count != targetCandleIndexes.Count)
            return;

        HashSet<int> playerSet = new HashSet<int>(playerLitCandles);
        HashSet<int> targetSet = new HashSet<int>(targetCandleIndexes);

        if (playerSet.SetEquals(targetSet))
        {
            if (!ritualCompleted)
            {
                ritualCompleted = true;
                Debug.Log("Candle Ritual Complete");

                if (ritualCompleteSound != null)
                    ritualCompleteSound.Play();

                if (hourglassLight2 != null)
                    hourglassLight2.SetActive(true); 
            }
        }
        else
        {
            FailRitual();
        }
    }

    void FailRitual()
    {
        Debug.Log("Ritual failed! All candles extinguished.");
        string correctCandles = string.Join(", ", targetCandleIndexes.ConvertAll(i => (i + 1).ToString()));
        Debug.Log("Correct candles were: " + correctCandles);

        playerLitCandles.Clear();

        foreach (ParticleSystem flame in candleFlames)
        {
            if (flame != null)
                flame.Stop();
        }
    }
}
