using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashbangWin : MonoBehaviour
{
    [Header("Flashbang Settings")]
    public Image flashImage;
    public AudioClip preFlashSound;
    public AudioClip flashSound;
    public AudioClip finalSound;
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

        StartCoroutine(PlayFlashSequence());
    }

    private IEnumerator PlayFlashSequence()
    {
        
        if (preFlashSound != null)
        {
            audioSource.clip = preFlashSound;
            audioSource.Play();
            yield return new WaitForSeconds(preFlashSound.length);
        }

        
        if (flashSound != null)
        {
            audioSource.PlayOneShot(flashSound);
        }

        
        yield return StartCoroutine(FadeInImage());

        
        if (flashSound != null)
        {
            yield return new WaitForSeconds(flashSound.length);
        }

        
        yield return StartCoroutine(FadeToBlack());

        
        if (finalSound != null)
        {
            audioSource.PlayOneShot(finalSound);
        }
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

        c.a = endAlpha;
        flashImage.color = c;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color startColor = flashImage.color;
        Color endColor = Color.black;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            flashImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        flashImage.color = endColor;
    }
}
