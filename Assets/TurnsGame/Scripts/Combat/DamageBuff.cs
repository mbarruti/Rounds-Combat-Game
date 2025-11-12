using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : Buff
{
    public float value;
    List<IEffect> effectList = new();

    public override object Use()
    {
        value = 0;
        List<IEffect> copy = new(effectList);
        foreach (var effect in copy)
        {
            effect.Apply();
        }

        return value;
    }

    public override void Apply(object valueT)
    {
        value = (float)valueT;
    }

    public override void Add(IEffect damageEffect)
    {
        effectList.Add(damageEffect);
    }

    public override void Remove(IEffect damageEffect)
    {
        effectList.Remove(damageEffect);
    }
}
