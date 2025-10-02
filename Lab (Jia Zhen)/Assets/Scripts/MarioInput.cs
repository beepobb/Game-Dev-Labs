using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MarioInput : MonoBehaviour
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
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;

    // Start is called before the first frame update

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
            Time.timeScale = 0.0f;
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

        if (playerInputActions.Player.Jump.triggered && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
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
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
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
    }
}
