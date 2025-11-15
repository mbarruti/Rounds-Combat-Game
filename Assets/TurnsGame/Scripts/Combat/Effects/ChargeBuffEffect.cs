using System;
using UnityEngine;
using static MyProject.Constants;

public class ChargeBuffEffect : IEffect
{
    public string Name => "Charged Attack Buff";
    public EffectTrigger Trigger { get; private set; } = CHARGED_ATTACK;
    public int MaxUses { get; private set; } = 1;
    public int Uses { get; private set; } = 0;

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} increases stats"));
    }
    public void Apply(CharacterManager user, CharacterManager target)
    {
        // TO-DO: make charged buffs depend on weapon
        if (Uses == MaxUses)
        {
            GetRemoved(user, target);
            return;
        }
        user.activeBuffs.BonusDamage += 0.4f;
        user.activeBuffs.Accuracy += 0.2f;
        Uses++;
    }

    public void GetRemoved(CharacterManager user, CharacterManager target)
    {
        user.activeBuffs.BonusDamage -= 0.4f;
        user.activeBuffs.Accuracy -= 0.2f;
        user.RemoveEffect(this);
    }
}
