using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float upSpeed = 10;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject enemies;
    [SerializeField] private JumpOverGoomba jumpOverGoomba;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator marioAnimator;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private AudioSource marioAudio;
    [SerializeField] private AudioClip marioDeath;
    [SerializeField] private float deathImpulse = 15;
    private SpriteRenderer marioSprite;
    private PlayerInput playerInputActions;
    private Rigidbody2D marioBody;
    private bool faceRightState = true;
    private bool onGroundState = true;

    // Jump related stuff
    [Header("Jump")]
    [SerializeField, Tooltip("Maximum number of jumps allowed")] private int maxJumps = 2;
    private int jumpCount = 0;

    [System.NonSerialized]
    public bool alive = true;

    void Awake()
    {
        playerInputActions = new PlayerInput();
        startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions.Player.Enable();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponentInChildren<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        Vector2 moveHorizontal = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (moveHorizontal != Vector2.zero)
        {
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                marioBody.AddForce(moveHorizontal * speed);
        }

        // stop
        if (moveHorizontal == Vector2.zero)
        {
            // stop
            marioBody.linearVelocity = new Vector2(0, marioBody.linearVelocity.y);
        }
    }

    void Update()
    {
        Vector2 moveHorizontal = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (moveHorizontal.x < 0 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (moveHorizontal.x > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));

        // Jump
        if (playerInputActions.Player.Jump.triggered && (onGroundState || jumpCount < maxJumps))
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpCount++;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
            marioAnimator.SetTrigger("isJumping");
            // Play sound immediately
            marioAudio.PlayOneShot(marioAudio.clip);
        }
    }

    public void RestartButtonCallback(int input)
    {
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        gameOverScoreText.text = "Score: " + jumpOverGoomba.score;
        gameOverPanel.SetActive(true);
        restartButton.SetActive(true);
        scoreText.gameObject.SetActive(false);
        // Disable player input
        playerInputActions.Player.Disable();
    }

    public void ResetGame()
    {
        // reset position
        Debug.Log("Resetting player position to " + startPosition);
        marioBody.transform.position = startPosition;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        // reset score
        jumpOverGoomba.score = 0;
        // hide game over text and button
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        scoreText.gameObject.SetActive(true);
        // Enable player input
        playerInputActions.Player.Enable();
        // Force idle animation
        marioAnimator.Play("mario-idle", 0);
        marioAnimator.ResetTrigger("isJumping");
        marioAnimator.ResetTrigger("onSkid");
        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // Reset block animations
        foreach (var block in FindObjectsByType<BaseBlock>(FindObjectsSortMode.None))
        {
            block.ResetBlock();
        }
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

            // play death animation
            marioAnimator.Play("mario-death", 0);
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }
    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        GameOver();
    }
}
