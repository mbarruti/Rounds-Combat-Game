using UnityEngine;

public interface IEffect
{
    string Name { get; }
    int Duration { get; }
    EffectTrigger Trigger { get; }
    void GetAdded();
    void Apply();
}

public enum EffectTrigger
{
    RoundStart,
    PerformAction,
    RoundEnd,
    Other
}