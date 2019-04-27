using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;

    public Vector3 offset = new Vector3(0f, 0f, 0f);

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position + offset;
    }
}
