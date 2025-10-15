using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public IntVariable gameScore;

    void Start()
    {
        Debug.Log("MainMenu Start called");
        gameScore.Value = 0; // reset score
        Debug.Log("Current high score: " + gameScore.previousHighestValue);
        UpdateHighScoreText();
    }

    void OnEnable()
    {
        UpdateHighScoreText();
        gameScore.onValueChanged.AddListener(UpdateHighScoreText);
    }

    void OnDisable()
    {
        gameScore.onValueChanged.RemoveListener(UpdateHighScoreText);
    }

    void UpdateHighScoreText()
    {
        highscoreText.text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
    }
}
