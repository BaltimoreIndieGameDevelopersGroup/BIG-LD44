using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoEnding : MonoBehaviour
{
    public string url = "http://pixelcrushers.com/big/loose_change/ending_scene.avi";
    public GameObject player;

    private VideoPlayer videoPlayer;

    IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield break;
        //Debug.Log("Playing " + url);
        //videoPlayer.url = url;
#endif
        // Play the video:
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        float elapsed = 0;
        while (!videoPlayer.isPrepared && elapsed < 8)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }
        videoPlayer.Play();
        player.SetActive(false);
    }
}
