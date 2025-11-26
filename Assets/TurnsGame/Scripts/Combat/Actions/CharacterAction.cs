using System;
using MyProject;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public abstract class CharacterActionSO
{
    [field: SerializeField] public ActionPriority Lead { get; private set; } = NONE;
    [field: SerializeField] public bool CanRecoverMeter { get; private set; }

    public abstract CharacterAction CreateAction();
}

[Serializable]
public class CharacterAction
{
    // private CharacterActionSO characterActionSO;
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
