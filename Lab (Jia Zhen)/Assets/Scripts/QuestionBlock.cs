using UnityEngine;

public class QuestionBlock : BaseBlock
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer blockRenderer; // the sprite renderer for the block
    [SerializeField] private Sprite usedBlockSprite;

    [SerializeField] private Animator blockAnimator;

    protected override void HitFromBelow()
    {
        if (blockAnimator) blockAnimator.enabled = false; // disable first
        if (blockRenderer) blockRenderer.sprite = usedBlockSprite;

        if (!used) SpawnCoin();
        base.HitFromBelow();
    }

    public override void ResetBlock()
    {
        base.ResetBlock();

        if (blockAnimator) blockAnimator.enabled = true; // re-enable animator
        blockAnimator.Play("question_block_blink", 0, 0f); // reset to blinking animation

    }
}

