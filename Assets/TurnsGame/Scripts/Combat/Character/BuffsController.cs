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

    // Other stats
    public float DmgReduction { get; set; }
}
