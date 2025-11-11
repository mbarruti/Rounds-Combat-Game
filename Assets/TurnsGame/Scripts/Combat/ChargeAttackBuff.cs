using UnityEngine;
using static MyProject.Constants;

public class ChargeAttackBuff : IEffect
{
    public string Name => "Charged Attack";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = EffectTrigger.Other;

    public void GetAdded(CharacterManager target)
    {
        target.activeBuffs[DAMAGE].Add(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{target.username} gets more damage"));
    }

    public void Apply(CharacterManager target)
    {
        Duration--;
        target.activeBuffs[DAMAGE].damageBuff += CHARGE_BUFF;
        if (Duration == 0) target.activeBuffs[DAMAGE].Remove(this);
    }
}