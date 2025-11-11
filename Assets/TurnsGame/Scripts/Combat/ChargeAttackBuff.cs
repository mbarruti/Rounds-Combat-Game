using UnityEngine;
using static MyProject.Constants;

public class ChargeAttackBuff : IEffect
{
    public string Name => "Charged Attack";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = EffectTrigger.Other;

    public void GetAdded(CharacterManager user)
    {
        user.activeBuffs[DAMAGE].Add(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} gets more damage"));
    }

    public void Apply(CharacterManager user)
    {
        Duration--;
        user.activeBuffs[DAMAGE].damageBuff += CHARGE_BUFF;
        if (Duration == 0) user.activeBuffs[DAMAGE].Remove(this);
    }
}