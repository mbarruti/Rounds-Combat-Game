using UnityEngine;

public class ActionContext
{
    public CharacterManager User { get; }
    public CharacterManager Target { get; }

    public ActionContext(CharacterManager user, CharacterManager target)
    {
        User = user;
        Target = target;
    }
}