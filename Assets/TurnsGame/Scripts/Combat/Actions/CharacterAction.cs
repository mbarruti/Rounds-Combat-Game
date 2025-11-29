using System;
using MyProject;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

[Serializable]
public class CharacterAction
{
    protected CharacterActionSO DataSO { get; set; }
    public string Name { get; protected set; } = "None";
    public ActionType Type { get; protected set; } = ActionType.NoType;
    public ActionPriority Lead { get; protected set; } = NONE;
    public float MeterCost { get; protected set; } = 0;
    public bool CanRecoverMeter { get; protected set; } = false;

    public CharacterAction(CharacterActionSO charActionSO)
    {
        DataSO = charActionSO;
        Name = DataSO.Name;
        Type = DataSO.Type;
        Lead = DataSO.Lead;
        MeterCost = DataSO.MeterCost;
        CanRecoverMeter = DataSO.CanRecoverMeter;
    }
    protected CharacterManager Player { get; set; }

    protected bool Completed { get; set; } = false;
    public event Action OnCompleted;

    public virtual void Execute(CharacterManager player, CharacterManager target){}

    protected void CompleteAction()
    {
        Completed = true;
        OnCompleted?.Invoke();
    }
}
