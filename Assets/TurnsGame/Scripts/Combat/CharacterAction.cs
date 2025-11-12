using UnityEngine;

//[System.Serializable]
//public abstract class Action
//{
//    public abstract void Execute(ActionContext context);
//}

[System.Serializable]
public class CharacterAction
{
    protected CharacterManager User { get; set; }
    protected CharacterAction LastAction { get; set; }
    protected CharacterAction NextAction { get; set; }

    public CharacterAction(CharacterManager user, CharacterAction lastAction)
    {
        User = user;
        LastAction = lastAction;
    }

    public virtual void Execute(CharacterManager target) { }
}
