using System;
using UnityEngine;
using static MyProject.Constants;

public class ChargeBuffEffect : IEffect
{
    public string Name => "Charged Attack Buff";
    public EffectTrigger Trigger { get; private set; } = CHARGED_ATTACK;
    public int MaxUses { get; private set; } = SINGLE_USE;
    public int Uses { get; private set; } = 0;

    public void Apply(CharacterManager user, CharacterManager target)
    {
        // TO-DO: make charged buffs depend on weapon
        if (Uses == 0)
        {
            user.activeBuffs.BonusDamage += 0.4f;
            user.activeBuffs.Accuracy += 0.2f;
            user.activeBuffs.Prowess += 0.2f;
            CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} increases stats"));
        }
        if (Uses < MaxUses) Uses++;
    }

    public void Consume(CharacterManager user, CharacterManager target)
    {
        if (Uses == MaxUses)
        {
            user.activeBuffs.BonusDamage -= 0.4f;
            user.activeBuffs.Accuracy -= 0.2f;
            user.activeBuffs.Prowess -= 0.2f;
            //user.RemoveEffect(this);
        }
    }
}
