using UnityEngine;

public class ResetHighScoreButtonController : MonoBehaviour, IInteractiveButton
{
    public IntVariable highScore;
    public void ButtonClick()
    {
        highScore.ResetHighestValue();
    }
}
