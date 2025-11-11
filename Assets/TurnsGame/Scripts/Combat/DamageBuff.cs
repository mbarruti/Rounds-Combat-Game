using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : Buff
{
    //public float damageBuff;

    List<IEffect> buffList = new();

    public override object Use(CharacterManager user)
    {
        damageBuff = 0;
        List<IEffect> copy = new(buffList);
        foreach (var buff in copy)
        {
            buff.Apply(user);
        }

        return damageBuff;
    }

    public override void Add(IEffect damageEffect)
    {
        buffList.Add(damageEffect);
    }

    public override void Remove(IEffect damageEffect)
    {
        buffList.Remove(damageEffect);
    }
}
