using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntTool : MonoBehaviour
{
    public int parameter;
    public UnityEvent<int> useInt;

    public void TriggerIntEvent()
    {

        useInt.Invoke(1); // safe to invoke even without callbacks

    }
}