using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;   
    public RenderTexture renderTexture; 

    void Start()
    {
#if UNITY_STANDALONE
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "defaultJumpscare.mp4");
        videoPlayer.url = videoPath;
#endif

        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}

