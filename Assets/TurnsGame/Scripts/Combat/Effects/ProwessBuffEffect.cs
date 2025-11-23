using UnityEngine;

public class ProwessBuffEffect : IEffect
{
    public float Value { get; private set; }
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

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Uses == 0)
        {
            user.activeBuffs.Prowess += Value;
            CombatUI.AddAnimation(CombatUI.Instance.WriteText
                ($"{user.username} increases prowess by {Value*100}%"));
        }
        if (Uses < MaxUses) Uses++;
    }

    public void Consume(CharacterManager user, CharacterManager target)
    {
        if (Uses == MaxUses)
        {
            user.activeBuffs.Prowess -= Value;
            //user.RemoveEffect(this);
        }
    }
}
