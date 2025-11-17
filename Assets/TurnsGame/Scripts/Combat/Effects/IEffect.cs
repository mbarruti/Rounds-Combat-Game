using UnityEngine;

public interface IEffect
{
    string Name { get; }
    EffectTrigger Trigger { get; }
    int MaxUses { get; }
    void GetAdded(CharacterManager user, CharacterManager target);
    void Apply(CharacterManager user, CharacterManager target);
    void GetRemoved(CharacterManager user, CharacterManager target);

    // TO-DO: Change order of code so that effects with max uses are removed from the list
            // and/or corresponding buff before the next call to ApplyEffects is done
}

public enum EffectTrigger
{
    Instant,
    RoundStart,
    PerformAction,
    RoundEnd,
    Attack,
    ChargedAttack,
    Tackle
}
