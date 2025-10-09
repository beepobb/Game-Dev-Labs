using System.Collections;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    [Header("Animations and Visuals")]
    [SerializeField] private Animator coinAnimator;

    [Header("Physics")]
    private Vector3 startPos;

    [Header("Audio")]
    [SerializeField] private AudioSource coinSpawnAudio;

    protected bool used = false;

    protected virtual void Awake()
    {
        startPos = transform.position;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // same code as before
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                HitFromBelow();
            }
        }
    }
    protected virtual void HitFromBelow()
    {
        if (used) return;
        BounceBlock();
        used = true;
    }

    protected void SpawnCoin()
    {
        if (coinAnimator)
        {
            coinAnimator.SetTrigger("blockHit");
            coinSpawnAudio.Play();
        }
    }

    public virtual void ResetBlock()
    {
        used = false;

        // Reset coin animation
        coinAnimator.Play("coin_idle", 0, 0f); // make sure your coin has an Idle animation
    }

    protected void BounceBlock()
    {
        StartCoroutine(BounceRoutine());
    }

    private IEnumerator BounceRoutine()
    {
        Vector3 top = startPos + Vector3.up * 0.2f; // how high block bounces
        float speed = 10f;

        // Move up
        while (transform.position.y < top.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, top, speed * Time.deltaTime);
            yield return null;
        }

        // Move back down
        while (transform.position.y > startPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = startPos; // snap safely at end
    }
}
