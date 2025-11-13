using UnityEngine;

public interface IEffect
{
    string Name { get; }
    int Duration { get; }
    EffectTrigger Trigger { get; }
    void GetAdded(CharacterManager user, CharacterManager target);
    void Apply(CharacterManager user, CharacterManager target);
}

public enum EffectTrigger
{
    RoundStart,
    PerformAction,
    RoundEnd,
    Other
}
