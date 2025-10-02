using NUnit.Framework;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator brickAnimator;
    [SerializeField] private GameObject coinPrefab; // Assign coin prefab
    [SerializeField] private AudioSource brickAudio;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip coinSound;

    [Header("Settings")]
    [SerializeField] private bool isReusable = false;

    // Internal state
    private bool isUsed = false;
    private bool isBouncing = false;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only react to player
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // If player hit from below (contact normal points downward onto block)
                if (contact.normal.y > 0.5f)
                {
                    TriggerBlock();
                    break;
                }
            }
        }

    }

    private void TriggerBlock()
    {
        // Dont trigger if used (and not reusable) or currently bouncing
        if ((isUsed && !isReusable) || isBouncing) return;

        // Play bounce animation
        isBouncing = true;
        brickAnimator.SetTrigger("onBounce");

        // Spawn a coin above the block (if assigned)
        if (coinPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1f;
            GameObject spawnedCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            Debug.Log($"Brick position: {transform.position}, coin position: {spawnPos}");
        }

        // Play Sound
        PlayBrickHitSound();

        // Mark block as used if not reusable
        if (!isReusable)
        {
            isUsed = true;
            brickAnimator.SetBool("isUsed", true); // Switch sprite to "used" block
        }
    }

    public void SetIsBouncing(bool boolean)
    {
        isBouncing = boolean;
    }

    private void PlayBrickHitSound()
    {
        if (coinPrefab != null)
        {
            // Play coin sound
            brickAudio.PlayOneShot(coinSound);
        }
        else
        {
            // Play regular brick hit sound
            brickAudio.PlayOneShot(hitSound);
        }
    }
}
