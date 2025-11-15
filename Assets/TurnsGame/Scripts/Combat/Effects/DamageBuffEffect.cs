using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class DamageBuffEffect : IEffect
{
    float Value { get; set; }
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

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{user.username} increases damage by {Value*100}%"));
    }

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Uses == MaxUses)
        {
            GetRemoved(user,target);
            return;
        }
        if (Uses == 0) user.activeBuffs.BonusDamage += Value;
        Uses++;
    }

    public void GetRemoved(CharacterManager user, CharacterManager target)
    {
        user.activeBuffs.BonusDamage -= Value;
        user.RemoveEffect(this);
    }
}
