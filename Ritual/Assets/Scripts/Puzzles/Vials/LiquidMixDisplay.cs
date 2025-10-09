using UnityEngine;

public class LiquidMixDisplay : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the LiquidMixer instance to receive events from.")]
    public LiquidMixer liquidMixer;

    [Tooltip("Assign 4 SpriteRenderers in order (slot 0 -> slot 3).")]
    public SpriteRenderer[] colorSlots = new SpriteRenderer[4];

    [Header("Settings")]
    public Color emptyColor = Color.white;

    private int currentIndex = 0;

    private void Awake()
    {
        ResetSlotsToEmpty();
    }

    private void OnEnable()
    {
        if (liquidMixer == null)
        {
            Debug.LogWarning("LiquidMixDisplay: liquidMixer not assigned in inspector.");
            return;
        }

        liquidMixer.OnColorAdded += HandleColorAdded;
        liquidMixer.OnMixCleared += HandleMixCleared;
        Debug.Log("LiquidMixDisplay: subscribed to liquidMixer events.");
    }

    private void OnDisable()
    {
        if (liquidMixer != null)
        {
            liquidMixer.OnColorAdded -= HandleColorAdded;
            liquidMixer.OnMixCleared -= HandleMixCleared;
        }
    }

    private void ResetSlotsToEmpty()
    {
        currentIndex = 0;
        if (colorSlots == null) return;

        for (int i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i] != null)
                colorSlots[i].color = emptyColor;
        }
    }

    private void HandleColorAdded(LiquidMixer.LiquidType type)
    {
        Debug.Log($"LiquidMixDisplay: HandleColorAdded received {type} (index {currentIndex})");

        if (colorSlots == null || colorSlots.Length == 0)
        {
            Debug.LogWarning("LiquidMixDisplay: colorSlots not set up.");
            return;
        }

        if (currentIndex >= colorSlots.Length)
        {
            Debug.Log("LiquidMixDisplay: all slots are full — ignoring extra color.");
            return;
        }

        var slot = colorSlots[currentIndex];
        if (slot == null)
        {
            Debug.LogWarning($"LiquidMixDisplay: slot {currentIndex} is null — skipping.");
            currentIndex++;
            return;
        }

        slot.color = GetColorFromType(type);
        currentIndex++;
    }

    private void HandleMixCleared()
    {
        Debug.Log("LiquidMixDisplay: HandleMixCleared -> resetting slots");
        ResetSlotsToEmpty();
    }

    private Color GetColorFromType(LiquidMixer.LiquidType type)
    {
        switch (type)
        {
            case LiquidMixer.LiquidType.Red: return Color.red;
            case LiquidMixer.LiquidType.Green: return Color.green;
            case LiquidMixer.LiquidType.Yellow: return Color.yellow;
            case LiquidMixer.LiquidType.Blue: return Color.blue;
            default: return emptyColor;
        }
    }
}
