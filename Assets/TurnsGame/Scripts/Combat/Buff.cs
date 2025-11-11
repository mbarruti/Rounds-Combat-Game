using Unity.VisualScripting;
using UnityEngine;

public enum BuffType
{
    BaseDamage,
    MeterDamage,
    Accuracy,
    Prowess,
    CounterChance,
    NumHits,
    BonusDamage
}

public abstract class Buff
{
    public float damageBuff;

    public abstract object Use(CharacterManager user);
    public abstract void Add(IEffect buffEffect);
    public abstract void Remove(IEffect buffEffect);
}
