using UnityEngine;
using static MyProject.Constants;

public class Charge : CharacterAction
{
    ChargeAttackBuff chargeBuff;
    public override void Execute(CharacterManager user, CharacterManager target)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} is charging an attack"));
        chargeBuff = new();
        chargeBuff.GetAdded(user);
    }
}
