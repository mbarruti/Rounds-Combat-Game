using System;
using MyProject;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public enum ActionType
{
    Attack,
    Block,
    WeaponSpecial,
    ShieldSpecial,
    NoType
}

public abstract class CharacterActionSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "None";
    [field: SerializeField] public ActionType Type { get; private set; } = ActionType.NoType;
    [field: SerializeField] public ActionPriority Lead { get; private set; } = NONE;
    [field: SerializeField] public float MeterCost { get; private set; } = 0;
    [field: SerializeField] public bool CanRecoverMeter { get; private set; } = true;

    public abstract CharacterAction CreateAction();

    protected virtual bool OnCanCreate(CharacterManager player) => true;
    public bool CanCreateAction(CharacterManager player)
    {
        if (player.shieldMeter.GetAvailableCharges() <= MeterCost) return false;
        if (!OnCanCreate(player)) return false;
        return true;
    }
}
