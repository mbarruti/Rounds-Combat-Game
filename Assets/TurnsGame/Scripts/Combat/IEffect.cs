using UnityEngine;

public interface IEffect
{
    string Name { get; }
    EffectTrigger Trigger { get; }
    int MaxUses { get; }
    void GetAdded(CharacterManager user, CharacterManager target);
    void Apply(CharacterManager user, CharacterManager target);
    void GetRemoved(CharacterManager user, CharacterManager target);
}

public enum EffectTrigger
{
    Instant,
    RoundStart,
    PerformAction,
    RoundEnd,
    Attack,
    ChargedAttack,
}
