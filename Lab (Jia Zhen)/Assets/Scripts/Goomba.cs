using System;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Animator goombaAnimator;
    private AudioSource goombaDeathAudio;
    private bool alive = true;
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
            goombaDeathAudio.Play();
            // Disable the collider to avoid further collisions
            GetComponent<Collider2D>().enabled = false;
            // Destroy the goomba after the death animation duration
            Destroy(gameObject, 0.5f); // Adjust the delay as per your animation length
        }
    }
}
