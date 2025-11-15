using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class Crushed : IEffect
{
    public string Name => "Crushed";
    public int MaxUses { get; private set; } = 1;
    public int Uses { get; private set; } = 0;

    public EffectTrigger Trigger { get; private set; } = ROUND_START;

    public void GetAdded(CharacterManager user, CharacterManager target)
    {
        user.AddEffect(this);
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} got crushed!"));
    }

    public void Apply(CharacterManager user, CharacterManager target)
    {
        Uses++;
        user.state = PlayerState.WAIT;
        user.action = null;
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{user.username} can't do anything", waitTime: 0));
        if (Uses == MaxUses) GetRemoved(user, target);
    }

    public void GetRemoved(CharacterManager user, CharacterManager target)
    {
        user.RemoveEffect(this);
    }
}
