using System;

namespace MyProject
{
    public enum ActionPriority
    {
        High = 1,
        Medium = 2,
        Low = 3,
        NullPriority = int.MaxValue,
    }

    public static class Constants
    {

        public const bool IS_PLAYER_ONE = true; // WHETHER THE USER IS PLAYER ONE OR PLAYER TWO

        // STATS

        public const float CHARGE_DAMAGE_BUFF = 0.2f; // BUFF TO CHARGED ATTACK

        public const float TACKLE_DMG_REDUCTION = 0.8f; // % DMG REDUCED FROM TACKLING

        public const float COUNTER_PROWESS_GAIN = 0.2f; // PROWESS GAINED FROM GETTING A COUNTER

        public const float COUNTER_PROWESS_LOSS = -1; // PROWESS LOST WHEN BEING COUNTERED

        public const float FORCED_PARRY_CHANCE = 1;

        // METER

        public const float HALF_CHARGE = 0.5f; // CHARGE IN RECOVERY

        public const float FULL_CHARGE = 1; // CHARGE AVAILABLE

        public const float PARRY_METER_LOSS = 1.5f; // METER LOST AFTER FAILED PARRY

        // EFFECTS

        public const int SINGLE_USE = 1; // EFFECT WITH ONE USE

        public const int INFINITE = -1; // EFFECT WITH INFINITE USES

        // EFFECT TRIGGERS

        public const EffectTrigger ROUND_START = EffectTrigger.RoundStart;

        public const EffectTrigger ROUND_END = EffectTrigger.RoundEnd;

        public const EffectTrigger ANY_ACTION = EffectTrigger.PerformAction;

        public const EffectTrigger ATTACK = EffectTrigger.Attack;

        public const EffectTrigger BLOCK = EffectTrigger.Block;

        public const EffectTrigger CHARGED_ATTACK = EffectTrigger.ChargedAttack;

        public const EffectTrigger TACKLE = EffectTrigger.Tackle;

        public const EffectTrigger NOTHING = EffectTrigger.Nothing;

        // ACTION PRIORITY

        public const ActionPriority HIGH = ActionPriority.High;

        public const ActionPriority MEDIUM = ActionPriority.Medium;

        public const ActionPriority LOW = ActionPriority.Low;

        public const ActionPriority NONE = ActionPriority.NullPriority;

        // public const BuffType BASE_DAMAGE = BuffType.BaseDamage;
        // public const BuffType METER_DAMAGE = BuffType.MeterDamage;
        // public const BuffType ACCURACY = BuffType.Accuracy;
        // public const BuffType PROWESS = BuffType.Prowess;
        // public const BuffType COUNTER_CHANCE = BuffType.CounterChance;
        // public const BuffType NUM_HITS = BuffType.NumHits;
        // public const BuffType DAMAGE = BuffType.BonusDamage;
    }
}
