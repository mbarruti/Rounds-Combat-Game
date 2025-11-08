using UnityEngine;

public interface IEffect
{
    string Name { get; }
    int Duration { get; }
    void Apply(CharacterManager target);
}