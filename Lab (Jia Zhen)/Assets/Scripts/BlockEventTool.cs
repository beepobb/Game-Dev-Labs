using UnityEngine;

public class BlockEventTool : MonoBehaviour
{
    void Awake()
    {
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(ResetChildBlocks);
    }

    public void ResetChildBlocks()
    {
        foreach (
            BaseBlock block in GetComponentsInChildren<BaseBlock>()
        )
        {
            block.ResetBlock();
        }
    }
}
