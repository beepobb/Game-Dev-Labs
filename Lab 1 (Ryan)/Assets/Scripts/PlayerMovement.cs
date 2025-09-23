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
    [SerializeField] private int maxJumps = 2;
    private int jumpCount = 0;
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    [SerializeField] private GameObject gameOverUIObject; // Should be disabled by default

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            Debug.Log("Right!");
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            Debug.Log("Left!");
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (Input.GetKeyDown("space") && (onGroundState || jumpCount < maxJumps))
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            jumpCount += 1;
            onGroundState = false;
        }
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            // stop
            marioBody.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            // Reset jumps
            onGroundState = true;
            jumpCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // GAME OVER
            Debug.Log("Collided with goomba!");
            GameOver();
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

    }
}