using UnityEngine;

public class BlockEventTool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
