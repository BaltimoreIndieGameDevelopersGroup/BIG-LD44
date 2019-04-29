using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : MonoBehaviour
{
    public float kDistanceX = 20f;
    public float kMoveDistance = 0.1f;

    public float kJumpForce = 400f;

    public float kDistToGround = 1f;

    Vector2 upAxis = new Vector2(0f, 1f);

    enum RatState
    {
        idle,
        walkingLeft,
        walkingRight,
        startJump,
        jumping
    }
    RatState state = RatState.idle;

    bool coroutineStarted = false;

    Vector2 initialPosition;
    Vector2 targetPosition;

    Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector2(-kDistanceX, 0f);
    }

    void Update()
    {
        if(state == RatState.idle)
        {
            if (!coroutineStarted)
                StartCoroutine(ChangeFromIdle());
        }
        else if (state == RatState.walkingLeft)
        {
            MoveLeft();
            if (gameObject.transform.position.x == targetPosition.x)
                state = RatState.walkingRight;
        }
        else if(state == RatState.walkingRight)
        {
            MoveRight();
            if (gameObject.transform.position.x == initialPosition.x)
                state = RatState.walkingRight;
        }
        else if (state == RatState.startJump)
        {
           
        }
        else if (state == RatState.jumping)
        {

        }
    }

    void FixedUpdate()
    {
        
        if (state == RatState.idle)
        {
            
        }
        else if (state == RatState.walkingLeft)
        {
            if (gameObject.transform.position.x <= targetPosition.x)
                state = RatState.walkingRight;
        }
        else if (state == RatState.walkingRight)
        {
            if (gameObject.transform.position.x >= initialPosition.x)
                state = RatState.startJump;
        }
        else if (state == RatState.startJump)
        {
            Vector2 totalForce = new Vector2(0, kJumpForce);
            rigidbody.AddForce(totalForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            state = RatState.jumping;
        }
        else if (state == RatState.jumping)
        {
            if(isGrounded())
                state = RatState.idle;
        }
    }

    bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -upAxis, kDistToGround /*+ 0.1f*/);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "ground")
            {
                return true;
            }
        }
        return false;
    }


    void MoveLeft()
    {
        Vector3 newPos = new Vector3(   transform.position.x - kMoveDistance,
                                        transform.position.y,
                                        transform.position.z);
        transform.position = newPos;
    }

    void MoveRight()
    {
        Vector3 newPos = new Vector3(   transform.position.x + kMoveDistance,
                                        transform.position.y,
                                        transform.position.z);
        transform.position = newPos;
    }

    IEnumerator ChangeFromIdle()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(5f);
        state = RatState.walkingLeft;
        coroutineStarted = false;
    }
}
