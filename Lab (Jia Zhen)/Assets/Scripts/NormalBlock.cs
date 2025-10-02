using UnityEngine;

public class NormalBlock : BaseBlock
{

    [SerializeField] private bool hasCoin;
    protected override void HitFromBelow()
    {
        if (used) return;
        BounceBlock();
        if (hasCoin)
        {
            SpawnCoin();
        }
        used = true;
    }
}
