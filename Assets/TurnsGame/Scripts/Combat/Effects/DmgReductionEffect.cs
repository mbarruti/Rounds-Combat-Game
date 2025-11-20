public class DmgReductionEffect : IEffect
{
    public float Value { get; private set; }
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
    }

    public void Consume(CharacterManager user, CharacterManager target)
    {
        Uses++;
        if (Uses == MaxUses)
        {
            user.activeBuffs.DmgReduction -= Value;
            //user.RemoveEffect(this);
        }
    }
}
