using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Crushed : IEffect
{
    public CharacterManager Target { get; private set; }

    public Crushed(CharacterManager target)
    {
        Target = target;
    }

    public string Name => "Crushed";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = EffectTrigger.RoundStart;

    public void GetAdded()
    {
        Target.AddEffect(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Target.username} got crushed!"));
    }

    public void Apply()
    {
        Duration--;
        Target.state = PlayerState.WAIT;
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Target.username} can't do anything", waitTime: 0));
        if (Duration == 0) Target.RemoveEffect(this);
    }
}
