using System;
using MyProject;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public enum ActionType
{
    Attack,
    Block,
    Special,
}

[Serializable]
public class CharacterAction
{
    // private CharacterActionSO characterActionSO;
    public ActionPriority Lead { get; protected set; } = NONE;
    public bool CanRecoverMeter { get; protected set; } = false;
    // public CharacterAction(){}

    protected CharacterActionSO DataSO { get; set; }
    protected CharacterManager Player { get; set; }
    public CharacterAction LastAction { get; protected set; }
    protected CharacterAction NextAction { get; set; }

    protected bool Completed { get; set; } = false;
    public event Action OnCompleted;

    public virtual void Execute(CharacterManager player, CharacterManager target){}

    protected void CompleteAction()
    {
        Completed = true;
        OnCompleted?.Invoke();
    }
}
