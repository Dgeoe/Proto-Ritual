using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;

    void Start()
    {
        
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.value = savedSensitivity;

        
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    void UpdateSensitivity(float value)
    {
        
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }
}
