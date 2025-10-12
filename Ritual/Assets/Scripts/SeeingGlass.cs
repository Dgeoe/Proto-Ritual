using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SeeingGlass : MonoBehaviour
{
    [Header("References")]
    public GameObject meshMonster;
    public Canvas uiCanvas;
    public GameObject SymbolsParent;
    public Volume globalVolume;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Post-Processing Targets")]
    public float vignetteTargetIntensity = 0.513f;
    public float bloomTargetIntensity = 0.18f;
    public float chromaticTargetIntensity = 0.67f;

    [Header("Light Sets (Point Lights with Colors)")]
    public Light[] diamondLights;    // 5 lights
    public Color[] diamondColors;    // colors for each light

    public Light[] hourglassLights;  // 5 lights
    public Color[] hourglassColors;

    public Light[] tridentLights;    // 5 lights
    public Color[] tridentColors;

    public Light[] helixLights;      // 5 lights
    public Color[] helixColors;

    private Light[][] lightSets;
    private Color[][] colorSets;

    private Color[] originalColors;

    private bool isActive = false;
    private float fadeTimer = 0f;

    private Vignette vignette;
    private Bloom bloom;
    private ChromaticAberration chromatic;
    private CanvasGroup uiCanvasGroup;

    public int chosenSetIndex; 

    void Awake()
    {
        
        lightSets = new Light[4][];
        lightSets[0] = diamondLights;
        lightSets[1] = hourglassLights;
        lightSets[2] = tridentLights;
        lightSets[3] = helixLights;

        colorSets = new Color[4][];
        colorSets[0] = diamondColors;
        colorSets[1] = hourglassColors;
        colorSets[2] = tridentColors;
        colorSets[3] = helixColors;

        
        int totalLights = 0;
        foreach (Light[] set in lightSets) totalLights += set.Length;

        originalColors = new Color[totalLights];
        int index = 0;
        foreach (Light[] set in lightSets)
        {
            foreach (Light l in set)
            {
                if (l != null)
                    originalColors[index++] = l.color;
            }
        }

        
        chosenSetIndex = Random.Range(0, lightSets.Length);
        switch (chosenSetIndex)
        {
            case 0: Debug.Log("Diamond Lights have been chosen"); break;
            case 1: Debug.Log("Hourglass Lights have been chosen"); break;
            case 2: Debug.Log("Trident Lights have been chosen"); break;
            case 3: Debug.Log("Helix Lights have been chosen"); break;
        }

        
        if (uiCanvas != null)
        {
            uiCanvasGroup = uiCanvas.GetComponent<CanvasGroup>();
            if (uiCanvasGroup == null)
                uiCanvasGroup = uiCanvas.gameObject.AddComponent<CanvasGroup>();

            uiCanvasGroup.alpha = 0f;
            uiCanvas.gameObject.SetActive(false);
        }

        
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet<Vignette>(out vignette);
            globalVolume.profile.TryGet<Bloom>(out bloom);
            globalVolume.profile.TryGet<ChromaticAberration>(out chromatic);

            if (vignette != null) vignette.intensity.value = 0f;
            if (bloom != null) bloom.intensity.value = 0f;
            if (chromatic != null) chromatic.intensity.value = 0f;
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleSeeingGlass();
        }

        if (fadeTimer > 0f)
        {
            float t = Mathf.Clamp01(1 - fadeTimer / fadeDuration);

            if (uiCanvasGroup != null)
                uiCanvasGroup.alpha = isActive ? t : 1 - t;

            if (vignette != null)
                vignette.intensity.value = isActive ? t * vignetteTargetIntensity : (1 - t) * vignetteTargetIntensity;
            if (bloom != null)
                bloom.intensity.value = isActive ? t * bloomTargetIntensity : (1 - t) * bloomTargetIntensity;
            if (chromatic != null)
                chromatic.intensity.value = isActive ? t * chromaticTargetIntensity : (1 - t) * chromaticTargetIntensity;

            fadeTimer -= Time.deltaTime;

            if (!isActive && fadeTimer <= 0f && uiCanvasGroup != null)
                uiCanvas.gameObject.SetActive(false);
        }
    }

    void ToggleSeeingGlass()
    {
        isActive = !isActive;
        fadeTimer = fadeDuration;

        
        if (meshMonster != null)
        {
            MeshRenderer mr = meshMonster.GetComponent<MeshRenderer>();
            if (mr != null)
                mr.enabled = !isActive;
        }

        
        if (isActive && uiCanvas != null)
            uiCanvas.gameObject.SetActive(true);

        if (SymbolsParent != null)
            SymbolsParent.SetActive(isActive);

        
        if (isActive)
            ActivateChosenLightSet();
        else
            RestoreOriginalLightColors();

        Debug.Log("Seeing Glass toggled: " + (isActive ? "ON" : "OFF"));
    }

    void ActivateChosenLightSet()
    {
        Light[] selectedLights = lightSets[chosenSetIndex];
        Color[] selectedColors = colorSets[chosenSetIndex];

        for (int i = 0; i < selectedLights.Length; i++)
        {
            if (selectedLights[i] != null && i < selectedColors.Length)
            {
                selectedLights[i].color = selectedColors[i];
                
            }
        }
    }

    void RestoreOriginalLightColors()
    {
        int index = 0;
        foreach (Light[] set in lightSets)
        {
            foreach (Light l in set)
            {
                if (l != null)
                    l.color = originalColors[index++];
            }
        }
    }
}
