using System;
using UnityEngine;

[Serializable]
public class BuffsController
{
    CharacterManager User { get; set; }

    public BuffsController (CharacterManager user)
    {
        User = user;
    }

    // Weapon buffs
    public float BaseDamage { get; set; } = 0;
    public float MeterDamage { get; set; } = 0;
    public float Accuracy { get; set; } = 0;
    public float Prowess { get; set; } = 0.5f;
    public float CounterChance { get; set; } = 0;
    public int NumHits { get; set; } = 0;
    public float BonusDamage { get; set; } = 0;

    // Shield buffs
    public float ParryChance { get; set; } = 0;

    // Other stats
    public float DmgReduction { get; set; } = 0;
}
