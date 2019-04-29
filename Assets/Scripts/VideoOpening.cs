using System.Collections;
using UnityEngine;

public class VideoOpening : MonoBehaviour
{
    public Canvas coverCanvas;

    IEnumerator Start()
    {
        coverCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        coverCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }
}
