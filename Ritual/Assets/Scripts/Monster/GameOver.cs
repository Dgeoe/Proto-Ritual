using UnityEngine;
using UnityEngine.UI; 

public class GameOver : MonoBehaviour
{
    [SerializeField] private float startTime = 120f; 
    private float currentTime;

    [SerializeField] private Text timerText; 

    private bool isRunning = false;

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

    private void OnTimerEnd()
    {
        Debug.Log("[JUMPSCARE HERE]!");
        Debug.Log("[SCREAM NOW]!");
    }

    public void ResetTimer() => currentTime = startTime;
    public void StopTimer() => isRunning = false;
    public void StartTimer() => isRunning = true;
}
