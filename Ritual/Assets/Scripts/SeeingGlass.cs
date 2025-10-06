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

    private bool isActive = false;
    private float fadeTimer = 0f;

    private Vignette vignette;
    private Bloom bloom;
    private ChromaticAberration chromatic;
    private CanvasGroup uiCanvasGroup;

    void Awake()
    {
        if (uiCanvas != null)
        {
            uiCanvasGroup = uiCanvas.GetComponent<CanvasGroup>();
            if (uiCanvasGroup == null)
            {
                uiCanvasGroup = uiCanvas.gameObject.AddComponent<CanvasGroup>();
            }
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
            {
                uiCanvas.gameObject.SetActive(false);
            }
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
        {
            uiCanvas.gameObject.SetActive(true);
        }

       
        if (SymbolsParent != null)
        {
            SymbolsParent.SetActive(isActive);
        }

        Debug.Log("Seeing Glass toggled: " + (isActive ? "ON" : "OFF"));
    }
}
