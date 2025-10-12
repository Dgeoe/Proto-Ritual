using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BrightnessSlider : MonoBehaviour
{
    
    public Slider brightnessSlider;

    
    public Volume postProcessVolume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        
        if (!postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogError("No ColorAdjustments found");
            return;
        }

        
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);
        colorAdjustments.postExposure.value = savedBrightness;

        
        brightnessSlider.value = savedBrightness;

        
        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
    }

    void UpdateBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
        }

        
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }
}
