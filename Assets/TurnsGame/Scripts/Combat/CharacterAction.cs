using UnityEngine;

//[System.Serializable]
//public abstract class Action
//{
//    public abstract void Execute(ActionContext context);
//}

[System.Serializable]
public class CharacterAction
{
    protected CharacterAction nextAction;

    public CharacterAction()
    {

    }

    public virtual void Execute(CharacterManager user, CharacterManager target)
    {
    }
}
