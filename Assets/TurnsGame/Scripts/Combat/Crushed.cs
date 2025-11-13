using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class Crushed : IEffect
{
    public string Name => "Crushed";
    public int Duration { get; private set; } = 1;

    public EffectTrigger Trigger { get; private set; } = ROUND_START;

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} got crushed!"));
    }

    public void Apply(CharacterManager user, CharacterManager target)
    {
        Duration--;
        user.state = PlayerState.WAIT;
        user.action = null;
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{user.username} can't do anything", waitTime: 0));
        if (Duration == 0) user.RemoveEffect(this);
    }
}
