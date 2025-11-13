using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class DamageBuff : IEffect
{
    float Value { get; set; }
    public DamageBuff(float value)
    {
        Value = value;
    }

    public string Name => "Damage Buff";
    public int Duration { get; private set; } = 1;
    public EffectTrigger Trigger { get; private set; } = ATTACK;

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.activeBuffs.Add(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} gets more damage"));
    }

    public void Apply(CharacterManager user, CharacterManager target)
    {
        if (Duration == 0)
        {
            user.activeBuffs.BonusDamage -= Value;
            user.activeBuffs.Remove(this);
        }
        else
        {
            Duration--;
            user.activeBuffs.BonusDamage += Value;
        }
    }
}
