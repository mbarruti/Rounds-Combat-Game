using UnityEngine;

public interface IEffect
{
    string Name { get; }
    EffectTrigger Trigger { get; }
    int MaxUses { get; }
    int Uses { get; }
    void Apply(CharacterManager user, CharacterManager target);
    void Consume(CharacterManager user, CharacterManager target);
}

public enum EffectTrigger
{
    RoundStart,
    PerformAction,
    RoundEnd,
    OnAttack,
    OnBlock,
    OnStance,
    Tackle,
    Nothing
}
