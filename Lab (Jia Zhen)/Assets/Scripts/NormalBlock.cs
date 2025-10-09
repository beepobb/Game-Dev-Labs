using UnityEngine;

public class NormalBlock : BaseBlock
{

    [SerializeField] private bool hasCoin;
    protected override void HitFromBelow()
    {
        if (hasCoin && !used)
        {
            Debug.Log("Spawning coin...");
            SpawnCoin();
        }
        base.HitFromBelow();
    }
}
