using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Crushed : IEffect
{
    public string Name => "Crushed";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = EffectTrigger.RoundStart;

    public void GetAdded(CharacterManager target)
    {
        target.AddEffect(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{target.username} got crushed!"));
    }

    public void Apply(CharacterManager target)
    {
        Duration--;
        target.state = PlayerState.WAIT;
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{target.username} can't do anything", waitTime: 0));
        if (Duration == 0) target.RemoveEffect(this);
    }
}
