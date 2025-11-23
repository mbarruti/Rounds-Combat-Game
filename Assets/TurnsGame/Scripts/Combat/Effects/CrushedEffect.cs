using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class CrushedEffect : IEffect
{
    public string Name => "Crushed";
    public int MaxUses { get; private set; } = SINGLE_USE;
    public int Uses { get; private set; } = 0;

    public EffectTrigger Trigger { get; private set; } = ROUND_START;

    public void Apply(CharacterManager user, CharacterManager target)
    {
        user.state = PlayerState.WAIT;
        user.action = null;
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{user.username} can't do anything", waitTime: 0));
        if (Uses < MaxUses) Uses++;
    }

    public void Consume(CharacterManager user, CharacterManager target)
    {
        // Uses++;
    }
}
