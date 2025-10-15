using UnityEngine;

public class PowerupBlock : BaseBlock
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer blockRenderer; // the sprite renderer for the block
    [SerializeField] private Sprite usedBlockSprite;
    [SerializeField] private Animator powerupAnimator;
    [SerializeField] private Animator blockAnimator;

    [Header("Audio")]
    [SerializeField] private AudioSource powerupSpawnAudio;

    protected override void HitFromBelow()
    {
        if (blockAnimator) blockAnimator.enabled = false; // disable first
        if (blockRenderer) blockRenderer.sprite = usedBlockSprite;

        if (!used && powerupAnimator)
        {
            Debug.Log("Powerup spawning animation triggered");
            powerupSpawnAudio.Play();
            powerupAnimator.SetTrigger("blockHit");
        }
        base.HitFromBelow();
    }

    public override void ResetBlock()
    {
        base.ResetBlock();

        if (blockAnimator) blockAnimator.enabled = true; // re-enable animator
        blockAnimator.Play("question_block_blink", 0, 0f); // reset to blinking animation

    }
}

