using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Awake()
    {
        // other instructions
        // Error: null reference exception
        // Right now GameManager.instance is null when EnemyManager.Awake runs
        // Fix: script execution order places GameManager.cs first
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().GameRestart();
        }
    }
}
