using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private bool moving = true;
    private bool jumpedState = false;
    [SerializeField] private int maxJumps = 2;
    private int jumpCount = 0;
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    [SerializeField] private GameObject gameOverUIObject; // Should be disabled by default

    [SerializeField] public Animator marioAnimator;

    // for audio
    public AudioSource marioAudio;
    public AudioClip marioDeath;

    // death
    public float deathImpulse = 30;

    // state
    [System.NonSerialized] public bool alive = true;

    // camera
    public Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        // Update animator state
        marioAnimator.SetBool("onGround", onGroundState); // parameter defined in Animator window

    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKeyDown("space") && (onGroundState || jumpCount < maxJumps))
        // {
        //     marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        //     jumpCount += 1;
        //     onGroundState = false;

        //     // update animator state
        //     marioAnimator.SetBool("onGround", onGroundState);
        // }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
            // float moveHorizontal = Input.GetAxisRaw("Horizontal");

            // if (Mathf.Abs(moveHorizontal) > 0)
            // {
            //     Vector2 movement = new Vector2(moveHorizontal, 0);
            //     // check if it doesn't go beyond maxSpeed
            //     if (marioBody.linearVelocity.magnitude < maxSpeed)
            //         marioBody.AddForce(movement * speed);
            // }

            // // stop
            // if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            // {
            //     // stop
            //     marioBody.linearVelocity = Vector2.zero;
            // }
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

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) && !onGroundState)
        {
            // Reset jumps
            onGroundState = true;
            jumpCount = 0;

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            // GAME OVER
            Debug.Log("Collided with goomba!");

            // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;

            //GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0.0f; // Freezes time
        gameOverUIObject.SetActive(true); // Show game over object
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
        // disable game over UI
        gameOverUIObject.SetActive(false); // Hide
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.01f, 0.0f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        jumpOverGoomba.score = 0;

        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
            Debug.Log("DEBUG1: " + eachChild.GetComponent<EnemyMovement>().startPosition);
        }

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

}