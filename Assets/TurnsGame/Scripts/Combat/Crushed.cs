using UnityEngine;

public class Crushed : IEffect
{
    public string Name => "Crushed";
    public int Duration { get; private set; } = 1;

    public void Apply(CharacterManager target)
    {
        Duration--;
        target.state = PlayerState.WAIT;
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{target.name} can't do anything"));
        if (Duration == 0) target.RemoveEffect(this);
    }
}
