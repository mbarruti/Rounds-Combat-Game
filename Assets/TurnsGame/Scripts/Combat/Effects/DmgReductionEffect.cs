public class DmgReductionEffect : IEffect
{
    float Value { get; set; }
    public int MaxUses { get; private set; }
    public EffectTrigger Trigger { get; private set; }
    public DmgReductionEffect(float value, int maxUses, EffectTrigger trigger)
    {
        Value = value;
        MaxUses = maxUses;
        Trigger = trigger;
    }

    public string Name => "Damage Reduction";
    public int Uses { get; private set; } = 0;

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Uses == 0) user.activeBuffs.DmgReduction += Value;
        Uses++;
        if (Uses == MaxUses)
        {
            user.RemoveEffect(this);
            return;
        }
    }

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText(
                $"{user.username} increases damage reduction by {Value*100}%"));
    }

    public void GetRemoved(CharacterManager user, CharacterManager target)
    {
        user.activeBuffs.DmgReduction -= Value;
        user.RemoveEffect(this);
    }
}
