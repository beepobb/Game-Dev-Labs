using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GoombaHead : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onStomped;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called in GoombaHead");

        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null && playerRb.linearVelocity.y <= 0f)
            {
                // Player is falling and has stomped the Goomba
                Debug.Log("Goomba stomped by player!");

                // Mario bounces (this could be made into a callback subscribed to onStomp)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
                playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

                // Trigger any onStomped logic
                onStomped?.Invoke();
            }
            else
            {
                // Player hit from side or below; do nothing (or could kill player if body collider)
                Debug.Log("Player hit head trigger from wrong direction.");
            }

        }
    }
}
