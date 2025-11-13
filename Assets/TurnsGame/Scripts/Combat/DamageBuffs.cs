using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffs : BuffsController
{
    public DamageBuffs (CharacterManager user) : base(user) { }

    List<IEffect> damageBuffsList = new();

    public override void ApplyAll()
    {
        BonusDamage = 0;
        List<IEffect> copy = new(damageBuffsList);
        foreach (var effect in copy)
        {
            effect.Apply(User, null);
        }
    }

    public override void Add(IEffect damageBuff)
    {
        damageBuffsList.Add(damageBuff);
    }

    public override void Remove(IEffect damageBuff)
    {
        damageBuffsList.Remove(damageBuff);
    }
}
