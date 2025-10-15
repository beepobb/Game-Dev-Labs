using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTool : MonoBehaviour
{
    public UnityEvent useEvent;

    public void TriggerEvent()
    {

        useEvent.Invoke(); // safe to invoke even without callbacks

    }
}
