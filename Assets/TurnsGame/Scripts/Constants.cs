
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

        public const float CHARGE_BUFF = 0.2f; // BUFF TO CHARGED ATTACK

        // BUFF TYPES

        public static readonly BuffType BASE_DAMAGE = BuffType.BaseDamage;
        public static readonly BuffType METER_DAMAGE = BuffType.MeterDamage;
        public static readonly BuffType ACCURACY = BuffType.Accuracy;
        public static readonly BuffType PROWESS = BuffType.Prowess;
        public static readonly BuffType COUNTER_CHANCE = BuffType.CounterChance;
        public static readonly BuffType NUM_HITS = BuffType.NumHits;
        public static readonly BuffType DAMAGE = BuffType.BonusDamage;
    }
}
