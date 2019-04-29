using System.Collections;
using UnityEngine;

public class VideoOpening : MonoBehaviour
{
    public Canvas coverCanvas;
    public GameObject player;

    IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield break;
#else
        player.SetActive(false);
        coverCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        coverCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
        player.SetActive(true);
#endif
    }
}
