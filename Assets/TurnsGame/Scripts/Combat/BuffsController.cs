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

public abstract class BuffsController
{
    protected CharacterManager User { get; set; }

    public BuffsController (CharacterManager user)
    {
        User = user;
    }

    public float BaseDamage { get; set; }
    public float MeterDamage { get; set; }
    public float Accuracy { get; set; }
    public float Prowess { get; set; }
    public float CounterChance { get; set; }
    public int NumHits { get; set; }
    public float BonusDamage { get; set; }

    public abstract void ApplyAll();
    //public abstract void Apply(object value);
    public abstract void Add(IEffect buffEffect);
    public abstract void Remove(IEffect buffEffect);
}
