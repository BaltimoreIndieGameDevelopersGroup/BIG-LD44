using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoOpening : MonoBehaviour
{
    public string url = "http://pixelcrushers.com/big/loose_change/opening_scene.avi";
    public Canvas coverCanvas;
    public GameObject player;

    private VideoPlayer videoPlayer;

    IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        //videoPlayer.url = url;
        //Debug.Log("Playing " + videoPlayer.url);
        gameObject.SetActive(false);
        yield break;
#endif
        // Hide the game:
        coverCanvas.gameObject.SetActive(true);
        player.SetActive(false);

        // Play the video:
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        float elapsed = 0;
        while (!videoPlayer.isPrepared && elapsed < 8) // Wait for it to prepare.
        {
            yield return null;
            elapsed += Time.deltaTime;
        }
        coverCanvas.gameObject.SetActive(false);
        videoPlayer.Play();
        elapsed = 0;
        while (videoPlayer.isPlaying && elapsed < 8) // Wait for it to finish playing.
        {
            yield return null;
            elapsed += Time.deltaTime;
        }
        gameObject.SetActive(false);
        player.SetActive(true);
    }
}
