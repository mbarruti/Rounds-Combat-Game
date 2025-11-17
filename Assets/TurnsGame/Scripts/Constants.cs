using System;

namespace MyProject
{
    public static class Constants
    {
        // COMBAT

        public const bool IS_PLAYER_ONE = true; // WHETHER THE USER IS PLAYER ONE OR PLAYER TWO

        public const float COUNTER_PROWESS_GAIN = 0.2f; // PROWESS GAINED FROM GETTING A COUNTER

        public const float COUNTER_PROWESS_LOSS = -1.0f; // PROWESS LOST WHEN BEING COUNTERED

        public const float HALF_CHARGE = 0.5f; // CHARGE IN RECOVERY

        public const float FULL_CHARGE = 1.0f; // CHARGE AVAILABLE

        public const float CHARGE_DAMAGE_BUFF = 0.2f; // BUFF TO CHARGED ATTACK

        public const float TACKLE_DMG_REDUCTION = 0.8f;

        public const int INFINITE = -1; // EFFECT HAS INFINITE DURATION

        // EFFECT TRIGGERS

        public const EffectTrigger INSTANT = EffectTrigger.Instant;

        public const EffectTrigger ROUND_START = EffectTrigger.RoundStart;

        public const EffectTrigger ROUND_END = EffectTrigger.RoundEnd;

        public const EffectTrigger ANY_ACTION = EffectTrigger.PerformAction;

        public const EffectTrigger ATTACK = EffectTrigger.Attack;

        public const EffectTrigger CHARGED_ATTACK = EffectTrigger.ChargedAttack;

        public const EffectTrigger TACKLE = EffectTrigger.Tackle;

        // public const BuffType BASE_DAMAGE = BuffType.BaseDamage;
        // public const BuffType METER_DAMAGE = BuffType.MeterDamage;
        // public const BuffType ACCURACY = BuffType.Accuracy;
        // public const BuffType PROWESS = BuffType.Prowess;
        // public const BuffType COUNTER_CHANCE = BuffType.CounterChance;
        // public const BuffType NUM_HITS = BuffType.NumHits;
        // public const BuffType DAMAGE = BuffType.BonusDamage;
    }
}
