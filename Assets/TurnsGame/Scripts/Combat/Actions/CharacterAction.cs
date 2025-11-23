using System;
using MyProject;
using UnityEngine;

//[System.Serializable]
//public abstract class Action
//{
//    public abstract void Execute(ActionContext context);
//}

[Serializable]
public class CharacterAction
{
    protected CharacterManager Player { get; set; }
    public ActionPriority Lead { get; protected set; }
    public bool CanRecoverMeter { get; protected set; }
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
        OnCompleted?.Invoke();
    }
}
