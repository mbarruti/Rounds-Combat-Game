using UnityEngine;

//[System.Serializable]
//public abstract class Action
//{
//    public abstract void Execute(ActionContext context);
//}

[System.Serializable]
public class Action
{
    public CharacterManager user;

    public virtual void Execute(CharacterManager user, CharacterManager target)
    {
        Debug.Log("HOLA TONTO");
    }
}
