using UnityEngine;

public class BrickAnimationEvents : MonoBehaviour
{
    [SerializeField] private BrickBehaviour brickBehaviour;

    public void OnBounceAnimationEnd()
    {
        // Set bouncing to false
        brickBehaviour.SetIsBouncing(false);
    }
}
