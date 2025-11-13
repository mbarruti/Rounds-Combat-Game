using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MyProject.Constants;

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

public class BuffsController
{
    CharacterManager User { get; set; }

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

    List<IEffect> baseDamageBuffs = new();
    List<IEffect> meterDamageBuffs = new();
    List<IEffect> accuracyBuffs = new();
    List<IEffect> prowessBuffs = new();
    List<IEffect> counterChanceBuffs = new();
    List<IEffect> numHitsBuffs = new();
    List<IEffect> damageBuffs = new();

    public void Consume(BuffType buffType)
    {
        switch (buffType)
        {
            case DAMAGE:
                BonusDamage = 0;
                List<IEffect> copy = new(damageBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
        }
    }

    public void Add(IEffect buff, BuffType buffType)
    {
        switch (buffType)
        {
            case DAMAGE:
                damageBuffs.Add(buff);
                break;
        }
    }

    public void Remove(IEffect buff, BuffType buffType)
    {
        switch (buffType)
        {
            case DAMAGE:
                damageBuffs.Remove(buff);
                break;
        }
    }
}
