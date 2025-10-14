using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable", order = 2)]
public class IntVariable : Variable<int>
{

    public int previousHighestValue;
    public UnityEvent onValueChanged;
    public override void SetValue(int value)
    {
        if (value > previousHighestValue) previousHighestValue = value;

        _value = value;
    }

    // overload
    public void SetValue(IntVariable value)
    {
        SetValue(value.Value);
    }

    public void ApplyChange(int amount)
    {
        this.Value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        ApplyChange(amount.Value);
    }

    public void ResetHighestValue()
    {
        previousHighestValue = 0;
        onValueChanged?.Invoke();
    }

}