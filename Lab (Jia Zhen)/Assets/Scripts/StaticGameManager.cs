// using UnityEngine;
// using UnityEngine.Events;

// public static class StaticGameManager
// {
//     // usage: StaticGameManager.gameStart.AddListener(<FunctionName>)
//     public static UnityEvent gameStart;
//     public static UnityEvent gameRestart;
//     public static UnityEvent<int> scoreChange;
//     public static UnityEvent gameOver;
//     public static UnityEvent damageMario;

//     public static IntVariable gameScore;

//     // usage: StaticGameManager.GameRestart()
//     public static void GameRestart()
//     {
//         // reset score
//         gameScore.Value = 0;
//         SetScore(gameScore.Value);
//         gameRestart.Invoke();
//         Time.timeScale = 1.0f;
//     }

//     public static void IncreaseScore(int increment)
//     {
//         gameScore.ApplyChange(increment);
//         SetScore(gameScore.Value);
//     }

//     // invoke the scorechange events
//     public static void SetScore(int score)
//     {
//         scoreChange.Invoke(score);
//     }

//     public static void GameOver()
//     {
//         Time.timeScale = 0.0f;
//     }

// }