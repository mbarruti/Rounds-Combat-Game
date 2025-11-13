using UnityEngine;

//[System.Serializable]
//public abstract class Action
//{
//    public abstract void Execute(ActionContext context);
//}

[System.Serializable]
public class CharacterAction
{
    protected CharacterManager Player { get; set; }
    protected CharacterAction LastAction { get; set; }
    protected CharacterAction NextAction { get; set; }

    public CharacterAction(CharacterManager user, CharacterAction lastAction)
    {
        Player = user;
        LastAction = lastAction;
    }

    public virtual void Execute(CharacterManager target){}
}
