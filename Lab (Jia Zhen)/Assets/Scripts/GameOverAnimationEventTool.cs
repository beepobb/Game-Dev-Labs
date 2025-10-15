using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverAnimationEventTool : MonoBehaviour
{
    public void TriggerGameOverEvent()
    {
        Debug.Log("TriggerGameOverEvent (Death Animation)");
        GameManager.instance.GameOver();
    }
}