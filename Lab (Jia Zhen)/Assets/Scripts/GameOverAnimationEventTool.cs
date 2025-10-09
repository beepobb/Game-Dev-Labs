using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverAnimationEventTool : MonoBehaviour
{
    public UnityEvent useGameOver;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TriggerGameOverEvent()
    {

        useGameOver.Invoke(); // safe to invoke even without callbacks

    }
}