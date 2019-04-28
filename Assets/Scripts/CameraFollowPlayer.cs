using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;

    public float kOffsetY = 3f;
    public float kOffsetZ = -10f;

    enum LookSide { Left, Center, Right };
    LookSide lookSide = LookSide.Right;
    public float kOffsetX = 0f;
    float lookOffsetX = 0f;
    float kInterpolationValue = 0.1f;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
        gameObject.transform.position = player.transform.position;
    }

    void LateUpdate()
    {
        CalculateLookOffset();
        CalculateCameraPosition();
    }

    void CalculateLookOffset()
    {
        if (lookSide == LookSide.Left)
            lookOffsetX = -kOffsetX;
        else if (lookSide == LookSide.Right)
            lookOffsetX = kOffsetX;
        else
            lookOffsetX = 0f;
        //if (lookSide == LookSide.Left)
        //    lookOffsetX = Mathf.Lerp(0f, player.transform.position.x - kOffsetX - transform.position.x, kInterpolationValue);
        //else if (lookSide == LookSide.Right)
        //    lookOffsetX = Mathf.Lerp(0f, player.transform.position.x + kOffsetX - transform.position.x, kInterpolationValue);
        //else
        //    lookOffsetX = Mathf.Lerp(0f, player.transform.position.x - transform.position.x, kInterpolationValue);
        
    }

    void CalculateCameraPosition()
    {
        float newX = player.transform.position.x /*+ kOffsetX*/;
        float newY = player.transform.position.y + kOffsetY;
        float newZ = player.transform.position.z + kOffsetZ;
        gameObject.transform.position = new Vector3(newX, newY, newZ);
    }

    public void lookLeft()
    {
        lookSide = LookSide.Left;
    }
    public void lookCenter()
    {
        lookSide = LookSide.Center;
    }
    public void lookRight()
    {
        lookSide = LookSide.Right;
    }
}
