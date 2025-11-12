using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class ChargeAttackBuff : IEffect
{
    public CharacterManager Target { get; private set; }

    public ChargeAttackBuff(CharacterManager target)
    {
        Target = target;
    }

    public string Name => "Charged Attack";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = EffectTrigger.Other;

    public void GetAdded()
    {
        Target.activeBuffs[DAMAGE].Add(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Target.username} gets more damage"));
    }

    public void Apply()
    {
        Duration--;
        Target.activeBuffs[DAMAGE].Apply(CHARGE_BUFF);
        if (Duration == 0) Target.activeBuffs[DAMAGE].Remove(this);
    }
}