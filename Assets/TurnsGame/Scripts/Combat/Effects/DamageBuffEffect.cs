using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class DamageBuffEffect : IEffect
{
    public float Value { get; private set; }
    public int MaxUses { get; private set; }
    public EffectTrigger Trigger { get; private set; }
    public DamageBuffEffect(float value, int maxUses, EffectTrigger trigger)
    {
        Value = value;
        MaxUses = maxUses;
        Trigger = trigger;
    }

    public string Name => "Damage Buff";
    public int Uses { get; private set; } = 0;

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Uses == 0) user.activeBuffs.BonusDamage += Value;
    }

    public void Consume(CharacterManager user, CharacterManager target)
    {
        Uses++;
        if (Uses == MaxUses)
        {
            user.activeBuffs.BonusDamage -= Value;
            //user.RemoveEffect(this);
        }
    }
}
