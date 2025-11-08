using UnityEngine;

public class Crushed : IEffect
{
    public string Name => "Crushed";
    public int Duration { get; private set; } = 1;

    public void Apply(CharacterManager target)
    {
        target.action = null;
    }
}
