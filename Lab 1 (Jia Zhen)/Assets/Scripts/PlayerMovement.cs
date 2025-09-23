using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float upSpeed = 10;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool onGroundState = true;
    private PlayerInput playerInputActions;
    private Rigidbody2D marioBody;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject restartButton;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    public GameObject gameOverPanel;
    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;

    // Jump related stuff
    [SerializeField, Tooltip("Maximum number of jumps allowed")] private int maxJumps = 2;
    private int jumpCount = 0;

    // Start is called before the first frame update

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Ground")) {
            jumpCount = 0;
            onGroundState = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Time.timeScale = 0.0f;
            backgroundMusic.Stop();
            gameOverMusic.Play();
            GameOver();
        }
    }

    void Awake()
    {
        playerInputActions = new PlayerInput();
    }

    void Start()
    {
        playerInputActions.Player.Enable();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponentInChildren<SpriteRenderer>();
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
        }
        else if (moveHorizontal.x > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }

        if (playerInputActions.Player.Jump.triggered && (onGroundState || jumpCount < maxJumps))
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpCount++;
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
    }

    public void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-7f, 2.0f, 0.0f);
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
        backgroundMusic.Play();
    }
}
