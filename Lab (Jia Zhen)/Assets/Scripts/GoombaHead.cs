using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GoombaHead : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onStomped;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D called in GoombaHead");

        if (collision.gameObject.CompareTag("Player"))
        {
            bool marioAlive = collision.gameObject.GetComponent<PlayerMovement>().alive;
            if (!marioAlive) return;

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb == null) return;

            // Get collision contact points
            ContactPoint2D contact = collision.contacts[0];

            // Check that collision happened from above (stomp)
            // Compare contact point y with Goomba's top y
            float goombaTop = GetComponent<Collider2D>().bounds.max.y;

            if (contact.point.y > goombaTop - 0.1f && playerRb.linearVelocity.y <= 0f)
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

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("OnTriggerEnter2D called in GoombaHead");

    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
    //         if (playerRb != null && playerRb.linearVelocity.y <= 0f)
    //         {
    //             // Player is falling and has stomped the Goomba
    //             Debug.Log("Goomba stomped by player!");

    //             // Mario bounces (this could be made into a callback subscribed to onStomp)
    //             playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
    //             playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

    //             // Trigger any onStomped logic
    //             onStomped?.Invoke();
    //         }
    //         else
    //         {
    //             // Player hit from side or below; do nothing (or could kill player if body collider)
    //             Debug.Log("Player hit head trigger from wrong direction.");
    //         }

    //     }
    // }
}
