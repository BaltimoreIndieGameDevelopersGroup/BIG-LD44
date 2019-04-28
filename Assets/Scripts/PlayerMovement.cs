using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const int GOLD_STATUS_DURATION = 5;

    Rigidbody2D rigidbody;
    public Vector2 kInitialSpeed = new Vector2(0,-10f);

    Vector2 upAxis = new Vector2(0f, 1f);
    Vector2 rightAxis = new Vector2(1f, 0f);

    public float kDistToGround = 1f;
    public float kForwardForce = 5.0f;
    public float kDecelerationForce = 10.0f;
    Vector2 forwardForce;

    public float kJumpForce = 400f;
    Vector2 jumpForce;

    public AudioClip landSound;
    private bool groundBelow_prev = false;

    private bool goldStatus = false;
    private HealthStatus health = HealthStatus.Quarter;
    bool changeCoin = false;

    CameraFollowPlayer cameraFollow;
    GameEvents gameEvents;
    AudioSource audioSource; // Rolling sound

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowPlayer>();
        gameEvents = GameObject.Find("GameManager").GetComponent<GameEvents>();
    }

    void Start()
    {
        rigidbody.velocity = kInitialSpeed;
    }

    void Update()
    {
        bool keyRight = Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow);
        bool keyLeft = Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow);
        bool keyUp = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("space");

        bool groundBelow = false;
        bool mudBelow = false;
        IsThereGround(ref groundBelow, ref mudBelow);

        if (mudBelow)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x * 0.90f, rigidbody.velocity.y);

        // Set volume of rolling sound based on ground speed:
        if (audioSource != null)
            audioSource.volume = groundBelow ? Mathf.Min(Mathf.Abs(rigidbody.velocity.x), 20f) / 20f : 0;

        // Landing sound:
        if (groundBelow && !groundBelow_prev && audioSource != null && landSound != null)
        {
            AudioSource.PlayClipAtPoint(landSound, Camera.main.transform.position, 1);
        }
        groundBelow_prev = groundBelow;

        //horizontal movement
        if (groundBelow && keyRight)
        {
            if(rigidbody.velocity.x > 0f)
                forwardForce = rightAxis * kForwardForce;
            else
                forwardForce = rightAxis * kDecelerationForce;
            cameraFollow.lookRight();
        }
        else if (groundBelow && keyLeft)
        {
            if (rigidbody.velocity.x < 0f)
                forwardForce = -rightAxis * kForwardForce;
            else
                forwardForce = -rightAxis * kDecelerationForce;
            cameraFollow.lookLeft();
        }
        else
        {
            forwardForce = new Vector2(0f, 0f);
        }

        //jump
        //TODO: only be able to jump when Player is touching ground
        if (groundBelow && keyUp)
        {
            jumpForce = upAxis * kJumpForce;
        }

        // update image state
        if (changeCoin && !goldStatus)
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

    void IsThereGround(ref bool ground, ref bool mud)
    {
        ground = mud = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -upAxis, kDistToGround /*+ 0.1f*/);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "mud")
            {
                mud = true;
                ground = true;
            }
            else if (hit.collider.tag == "ground")
            {
                ground = true;
            }
        }

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("collide with " + collider.name);
        Obstacle ob = collider.gameObject.GetComponent<Obstacle>();
        if (ob == null)
            return;
        if (ob.collisionSound != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(ob.collisionSound, Camera.main.transform.position, 1);
        }
        if (ob.state == (int)ObstacleState.Death)
        {
            gameEvents.PlayerDied();
        }
        else if (ob.state == (int)ObstacleState.Damage)
        {
            if (collider.gameObject.GetComponent<Obstacle>().isDestroyed)
            {
                Destroy(collider.gameObject);
            }
            StartCoroutine(Flasher());
            UpdateHealthState(false);
        }
        else if (ob && ob.state == (int)ObstacleState.Health)
        {
            if (collider.gameObject.GetComponent<Obstacle>().isDestroyed)
            {
                Destroy(collider.gameObject);
            }
            if(ob.specialCase == "gold")
            {
                StartCoroutine(GoGoldMode());
            }
            else
            {
                UpdateHealthState(true);
            }
        }
    }

    // Functions to be used as Coroutines MUST return an IEnumerator
    IEnumerator Flasher()
    {
        Debug.Log("flashing");
        Renderer renderer = GetComponent<Renderer>();
        for (int i = 0; i < 5; i++)
        {
            renderer.material.color = Color.white;
            yield return new WaitForSeconds(.1f);
            renderer.material.color = Color.gray;
            yield return new WaitForSeconds(.1f);
        }
        renderer.material.color = Color.white;
    }

    IEnumerator GoGoldMode()
    {
        Debug.Log("gold");
        goldStatus = true;
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Sprite myHealthCoin = Resources.Load("dollar", typeof(Sprite)) as Sprite;
        this.GetComponent<SpriteRenderer>().sprite = myHealthCoin;
        yield return new WaitForSeconds(GOLD_STATUS_DURATION);
        goldStatus = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = currentSprite;
    }
}
