using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float startTime = 120f;
    private float currentTime;

    [SerializeField] private Text timerText;
    private bool isRunning = false;

    [Header("Jumpscare Settings")]
    [SerializeField] private AudioSource jumpscareAudio; 
    [SerializeField] private float delayBeforeNextScene = 0.5f; 

    void Start()
    {
        currentTime = startTime;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void OnTimerEnd()
    {
        StartCoroutine(JumpscareSequence());
    }

    private IEnumerator JumpscareSequence()
    {
      
        if (jumpscareAudio != null)
        {
            jumpscareAudio.Play();
            
            yield return new WaitForSeconds(jumpscareAudio.clip.length + delayBeforeNextScene);
        }
        else
        {
            Debug.LogWarning("No jumpscare audio assigned");
        }

        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene found");
        }
    }

    public void ResetTimer() => currentTime = startTime;
    public void StopTimer() => isRunning = false;
    public void StartTimer() => isRunning = true;
}
