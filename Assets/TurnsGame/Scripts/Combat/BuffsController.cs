using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MyProject.Constants;

public class BuffsController
{
    CharacterManager User { get; set; }

    public BuffsController (CharacterManager user)
    {
        User = user;
    }

    List<IEffect> buffs = new();

    // Weapon buffs
    public float BaseDamage { get; set; }
    public float MeterDamage { get; set; }
    public float Accuracy { get; set; }
    public float Prowess { get; set; }
    public float CounterChance { get; set; }
    public int NumHits { get; set; }
    public float BonusDamage { get; set; }

    // Shield buffs
    public float ParryChance { get; set; }

    public void Consume(EffectTrigger trigger)
    {
        List<IEffect> copy = new(buffs);

        foreach (var buff in copy)
        {
            if (buff.Trigger == trigger)
                buff.Apply(User, null);
        }
    }

    public void Add(IEffect buff)
    {
        buffs.Add(buff);
    }

    public void Remove(IEffect buff)
    {
        buffs.Remove(buff);
    }
}
