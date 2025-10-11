using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;
    private bool moving = false;

    [Header("References")]
    [SerializeField] private GameObject enemies;
    [SerializeField] private JumpOverGoomba jumpOverGoomba;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator marioAnimator;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private AudioSource marioAudio;
    [SerializeField] private AudioClip marioDeath;
    private SpriteRenderer marioSprite;
    private Rigidbody2D marioBody;
    private bool faceRightState = true;
    private bool onGroundState = true;

    // Jump related stuff
    [Header("Jump")]
    private bool jumpedState = false;
    [SerializeField, Tooltip("Maximum number of jumps allowed")] private int maxJumps = 2;
    private int jumpCount = 0;

    [System.NonSerialized]
    public bool alive = true;

    // Start is called before the first frame update

    void Awake()
    {
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void Start()
    {
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        startPosition = gameConstants.marioStartingPosition;
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponentInChildren<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // public void SetStartingPosition(Scene current, Scene next)
    // {
    //     if (next.name == "World-1-2")
    //     {
    //         // change the position accordingly in your World-1-2 case
    //         this.transform.position = startPosition;
    //     }
    // }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        // if move action is canceled it will be 0 -> Move(0) will be called
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        Debug.Log("MoveCheck called with value: " + value);
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && (onGroundState || jumpCount < maxJumps))
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpCount += 1;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
            marioAnimator.SetTrigger("isJumping");
            marioAudio.PlayOneShot(marioAudio.clip);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(30 * upSpeed * Vector2.up, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    public void RestartButtonCallback(int input)
    {
        // reset everything
        GameRestart();
        // resume time
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
    }
    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = startPosition;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (((collisionLayerMask & (1 << collision2D.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            jumpCount = 0;
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") & alive)
        {
            Debug.Log("Collided with goomba!");
            // TODO: check that goomba is not stomped on i.e. still alive
            // If goomba is alive, mario dies from side collision
            // Get goomba's alive state
            Goomba goomba = other.gameObject.GetComponent<Goomba>();
            Debug.Log("Goomba alive state: " + (goomba != null ? goomba.GetAliveState().ToString() : "Goomba component not found"));
            if (goomba != null && goomba.GetAliveState())
            {
                Die();
            }
        }
    }

    public void Die()
    {
        // Ideally, Mario is only receiver, this gets called when colliding with goomba (goomba handles the collision detection)
        if (!alive) return;
        alive = false;
        // death animation
        marioAnimator.Play("mario-death", 0);
        marioAudio.PlayOneShot(marioDeath);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }
}
