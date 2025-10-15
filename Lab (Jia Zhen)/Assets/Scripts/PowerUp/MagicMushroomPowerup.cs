
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    [SerializeField] private AudioSource powerupCollectAudio;
    // setup this object's type
    // instantiate variables

    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            if (powerupCollectAudio)
            {
                Debug.Log("Playing powerup collect sound");
                AudioSource.PlayClipAtPoint(powerupCollectAudio.clip, transform.position);
            }
            Debug.Log("Powerup collected by Player!");
            // then destroy powerup (optional)
            DestroyPowerup();

        }
        else if (col.gameObject.layer == 6) // else if hitting obstacles, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // Step 1: Enable collider and make body dynamic (should be static at first)
        col.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        // Step 2: Wait until the next physics update
        yield return new WaitForNextFrameUnit();

        // Step 3: Now the Rigidbody is dynamic, apply a force
        rigidBody.AddForce(new Vector2(3f, 2f), ForceMode2D.Impulse); // move to the right
    }



    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

    }
}