using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject next;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // This method will be called when the video finishes
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video has finished playing.");
        next.SetActive(true);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}