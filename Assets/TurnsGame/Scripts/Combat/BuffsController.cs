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

    // Weapon buffs
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

    // Shield buffs
    public float ParryChance { get; set; }

    List<IEffect> parryChanceBuffs = new();

    public void Consume(BuffType buffType)
    {
        List<IEffect> copy = new();
        switch (buffType)
        {
            case BASE_DAMAGE:
                BaseDamage = 0;
                copy = new(baseDamageBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case METER_DAMAGE:
                MeterDamage = 0;
                copy = new(meterDamageBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case ACCURACY:
                Accuracy = 0;
                copy = new(accuracyBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case PROWESS:
                Prowess = 0;
                copy = new(prowessBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case COUNTER_CHANCE:
                CounterChance = 0;
                copy = new(counterChanceBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case NUM_HITS:
                NumHits = 0;
                copy = new(numHitsBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
            case DAMAGE:
                BonusDamage = 0;
                copy = new(damageBuffs);
                foreach (var buff in copy) buff.Apply(User, null);
                break;
        }
    }

    public void Add(IEffect buff, BuffType buffType)
    {
        switch (buffType)
        {
            case BASE_DAMAGE:
                baseDamageBuffs.Add(buff);
                break;
            case METER_DAMAGE:
                meterDamageBuffs.Add(buff);
                break;
            case ACCURACY:
                accuracyBuffs.Add(buff);
                break;
            case PROWESS:
                prowessBuffs.Add(buff);
                break;
            case COUNTER_CHANCE:
                counterChanceBuffs.Add(buff);
                break;
            case NUM_HITS:
                numHitsBuffs.Add(buff);
                break;
            case DAMAGE:
                damageBuffs.Add(buff);
                break;
        }
    }

    public void Remove(IEffect buff, BuffType buffType)
    {
        switch (buffType)
        {
            case BASE_DAMAGE:
                baseDamageBuffs.Remove(buff);
                break;
            case METER_DAMAGE:
                meterDamageBuffs.Remove(buff);
                break;
            case ACCURACY:
                accuracyBuffs.Remove(buff);
                break;
            case PROWESS:
                prowessBuffs.Remove(buff);
                break;
            case COUNTER_CHANCE:
                counterChanceBuffs.Remove(buff);
                break;
            case NUM_HITS:
                numHitsBuffs.Remove(buff);
                break;
            case DAMAGE:
                damageBuffs.Remove(buff);
                break;
        }
    }
}
