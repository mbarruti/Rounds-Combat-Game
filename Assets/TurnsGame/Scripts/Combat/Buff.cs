using System.Collections.Generic;
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
    //public object buff;
    //protected Dictionary<BuffType, IEffect> activeBuffs;
    public abstract object Use();
    public abstract void Apply(object value);
    public abstract void Add(IEffect buffEffect);
    public abstract void Remove(IEffect buffEffect);
}
