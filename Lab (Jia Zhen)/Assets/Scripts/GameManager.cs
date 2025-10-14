using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public IntVariable gameScore;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameScore.Value = 0;
        gameStart.Invoke();
        SetScore();
    }

    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        SetScore();
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore()
    {
        Debug.Log("Increase Score");
        gameScore.ApplyChange(1);
        SetScore();
    }

    public void SetScore()
    {
        scoreChange.Invoke(gameScore.Value);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}