using System;
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
    public CharacterAction LastAction { get; protected set; }
    protected CharacterAction NextAction { get; set; }

    protected bool Completed { get; set; } = false;
    public event Action OnCompleted;

    public CharacterAction(CharacterManager user, CharacterAction lastAction)
    {
        Player = user;
        LastAction = lastAction;
    }

    public virtual void Execute(CharacterManager target){}

    protected void CompleteAction()
    {
        Completed = true;
        OnCompleted?.Invoke();   // avisar a quienes están escuchando
    }
}
