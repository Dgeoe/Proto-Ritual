using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AudioSource))]
public class NextScene : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    public float hoverScale = 1.1f;    
    public float hoverSpeed = 10f;     

    [Header("Click Animation Settings")]
    public float slowScaleDuration = 1.5f;     
    public float fastScaleMultiplier = 20f;    
    public float fastScaleDuration = 0.2f;     
    public float waitAfterFullScreen = 5f;     

    [Header("TextMeshPro Fade")]
    public TextMeshProUGUI fadeText;           
    public float fadeDuration = 1f;            
    public float holdDuration = 1f;            

    [Header("Audio")]
    public AudioClip clickSound;               
    private AudioSource audioSource;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isClicked = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        audioSource = GetComponent<AudioSource>();

        if (fadeText != null)
        {
            fadeText.gameObject.SetActive(false); 
        }
    }

    void Update()
    {
        if (!isClicked) 
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * hoverSpeed);
        }
    }

    
    public void LoadNextScene()
    {
        if (!isClicked)
        {
            isClicked = true;

            
            if (clickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            StartCoroutine(ClickAnimationAndLoad());
        }
    }

    private System.Collections.IEnumerator ClickAnimationAndLoad()
    {
        
        Vector3 slowTarget = transform.localScale * hoverScale;
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < slowScaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, slowTarget, elapsed / slowScaleDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = slowTarget;

        
        startScale = transform.localScale;
        Vector3 fullScreenScale = originalScale * fastScaleMultiplier;
        elapsed = 0f;
        while (elapsed < fastScaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, fullScreenScale, elapsed / fastScaleDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = fullScreenScale;

        
        if (fadeText != null)
        {
            fadeText.gameObject.SetActive(true);
            CanvasGroup cg = fadeText.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = fadeText.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
            cg.blocksRaycasts = false; 

            
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                cg.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            cg.alpha = 1f;

            
            yield return new WaitForSeconds(holdDuration);

            
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                cg.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            cg.alpha = 0f;
            fadeText.gameObject.SetActive(false);
        }

        
        yield return new WaitForSeconds(waitAfterFullScreen);

        
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextIndex = currentIndex + 1;
        if (nextIndex >= totalScenes)
            nextIndex = 0; 
        SceneManager.LoadScene(nextIndex);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
            targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
            targetScale = originalScale;
    }
}
