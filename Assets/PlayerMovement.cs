using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float kInitialSpeedY = 1f;

    Vector2 upAxis = new Vector2(0f, 1f);
    Vector2 rightAxis = new Vector2(1f, 0f);

    public float kForwardForce = 1.0f;
    Vector2 forwardForce;

    public float kJumpForce = 1.0f;
    Vector2 jumpForce;

    public bool goldStatus = false;
    private HealthStatus health = HealthStatus.Quarter;
    bool changeCoin = false;

    CameraFollowPlayer cameraFollow;
    GameEvents gameEvents;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowPlayer>();
        gameEvents = GameObject.Find("GameManager").GetComponent<GameEvents>();
    }

    void Start()
    {
        rigidbody.velocity = new Vector2(0f, kInitialSpeedY);
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
            cameraFollow.lookRight();
        }
        else if (keyLeft)
        {
            forwardForce = -rightAxis * kForwardForce;
            cameraFollow.lookLeft();
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

        // update image state
        if (changeCoin)
        {
            Sprite myHealthCoin = null;
            switch (health)
            {
                case HealthStatus.Quarter:
                    myHealthCoin = Resources.Load("quarter", typeof(Sprite)) as Sprite;
                    this.GetComponent<SpriteRenderer>().sprite = myHealthCoin;
                    break;
                case HealthStatus.Nickel:
                    myHealthCoin = Resources.Load("nickel", typeof(Sprite)) as Sprite;
                    this.GetComponent<SpriteRenderer>().sprite = myHealthCoin;
                    break;
                case HealthStatus.Dime:
                    myHealthCoin = Resources.Load("dime", typeof(Sprite)) as Sprite;
                    this.GetComponent<SpriteRenderer>().sprite = myHealthCoin;
                    break;
                case HealthStatus.Penny:
                    myHealthCoin = Resources.Load("penny", typeof(Sprite)) as Sprite;
                    this.GetComponent<SpriteRenderer>().sprite = myHealthCoin;
                    break;
            }
            this.changeCoin = false;
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
            gameEvents.PlayerDied();
        }
        else
        {
            this.health += sum;
            this.changeCoin = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collide");
        if(collision.tag == "damage")
        {
            UpdateHealthState(false);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "health")
        {
            UpdateHealthState(true);
            Destroy(collision.gameObject);
        }
    }
}
