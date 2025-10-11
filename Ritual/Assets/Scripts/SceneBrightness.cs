using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SceneBrightness : MonoBehaviour
{
    [Header("References")]
    public Volume postProcessVolume;
    public Slider brightnessSlider; 

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        
        if (!postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("No ColorAdjustments found");
            return;
        }

        
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);
        colorAdjustments.postExposure.value = savedBrightness;

        
        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
            brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
        }
    }

    
    public void UpdateBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("Brightness", value);
            PlayerPrefs.Save();
        }
    }
}
