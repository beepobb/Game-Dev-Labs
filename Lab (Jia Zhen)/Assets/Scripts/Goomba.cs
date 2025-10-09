using System;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Animator goombaAnimator;
    private AudioSource goombaDeathAudio;
    private bool alive = true;

    // Audio settings
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;
    [SerializeField] private float minVolume = 0.9f;
    [SerializeField] private float maxVolume = 1.0f;

    void Start()
    {
        goombaAnimator = GetComponent<Animator>();
        goombaDeathAudio = GetComponent<AudioSource>();
    }
    public void SetAliveState()
    {
        alive = false;
        goombaAnimator.SetTrigger("Dead");
    }
    public bool GetAliveState()
    {
        return alive;
    }
    public void OnStomped()
    {
        if (alive)
        {
            SetAliveState();
            goombaAnimator.SetTrigger("Dead");
            PlayStompSound();
            // Disable the collider to avoid further collisions
            GetComponent<Collider2D>().enabled = false;
            // Destroy the goomba after the death animation duration
            Destroy(gameObject, 0.5f); // Adjust the delay as per your animation length
        }
    }

    private void PlayStompSound()
    {
        // Randomize pitch before playing
        goombaDeathAudio.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        goombaDeathAudio.volume = UnityEngine.Random.Range(minVolume, maxVolume);
        goombaDeathAudio.Play();
    }
}
