using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;

    Vector2 upAxis = new Vector2(0f, 1f);
    Vector2 rightAxis = new Vector2(1f, 0f);

    public float kForwardForce = 1.0f;
    Vector2 forwardForce;

    public float kJumpForce = 1.0f;
    Vector2 jumpForce;

    public bool goldStatus = false;
    private HealthStatus health = HealthStatus.Quarter;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        bool keyRight = Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow);
        bool keyLeft = Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow);
        bool keyUp = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("space");

        //horizontal movement
        if (keyRight)
        {
            forwardForce = rightAxis * kForwardForce;
        }
        else if (keyLeft)
        {
            forwardForce = -rightAxis * kForwardForce;
        }
        else
        {
            forwardForce = new Vector2(0f, 0f);
        }

        //jump
        //TODO: only be able to jump when Player is touching ground
        if (keyUp)
        {
            jumpForce = upAxis * kJumpForce;
        }
        
    }

    void FixedUpdate()
    {
        Vector2 totalForce = forwardForce + jumpForce;
        rigidbody.AddForce(totalForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        jumpForce = new Vector2(0f, 0f);
    }

    void UpdateHealthState(bool increase)
    {
        int sum = 0;
        if (increase)
        {
            sum = 1;
        }
        else
        {
            sum = -1;
        }

        if((this.health+sum) > HealthStatus.MaxHealth)
        {
            // Health is maxed...do nothing
        }
        else if ((this.health+sum) < HealthStatus.MinHealth)
        {
            // you dead!!!
            // TODO: Handle death scenario
        }
        else
        {
            this.health += sum;
        }
    }
}
