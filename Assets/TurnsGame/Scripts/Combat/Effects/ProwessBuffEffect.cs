using UnityEngine;

public class ProwessBuffEffect : IEffect
{
    float Value { get; set; }
    public int MaxUses { get; private set; }
    public EffectTrigger Trigger { get; private set; }
    public ProwessBuffEffect(float value, int maxUses, EffectTrigger trigger)
    {
        Value = value;
        MaxUses = maxUses;
        Trigger = trigger;
    }

    public string Name => "Prowess Buff";
    public int Uses { get; private set; } = 0;

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{user.username} increases prowess by {Value*100}%"));
    }

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Uses == 0) user.activeBuffs.Prowess += Value;
        Uses++;
        if (Uses == MaxUses)
        {
            user.RemoveEffect(this);
            return;
        }
    }

    public void GetRemoved(CharacterManager user, CharacterManager target)
    {
        //user.activeBuffs.Prowess -= Value;
        user.RemoveEffect(this);
    }
}
