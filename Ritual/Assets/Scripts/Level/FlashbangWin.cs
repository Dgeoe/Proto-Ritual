using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashbangWin : MonoBehaviour
{
    [Header("Flashbang Settings")]
    public Image flashImage;          
    public AudioClip flashSound;      
    public float fadeDuration = 5f;   

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }

    public void TriggerFlashbang()
    {
        if (flashImage == null)
        {
            Debug.LogWarning("FlashbangWin: No UI Image assigned!");
            return;
        }

        if (flashSound != null)
            audioSource.PlayOneShot(flashSound);

        StartCoroutine(FadeInImage());
    }

    private IEnumerator FadeInImage()
    {
        float elapsed = 0f;
        Color c = flashImage.color;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            flashImage.color = c;
            yield return null;
        }
        c.a = 1f;
        flashImage.color = c;
    }
}
