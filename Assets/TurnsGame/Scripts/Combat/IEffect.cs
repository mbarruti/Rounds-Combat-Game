using UnityEngine;

public interface IEffect
{
    string Name { get; }
    int Duration { get; }
    EffectTrigger Trigger { get; }
    void GetAdded(CharacterManager target);
    void Apply(CharacterManager target);
}

public enum EffectTrigger
{
    RoundStart,
    PerformAction,
    RoundEnd
}