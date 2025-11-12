using UnityEngine;
using static MyProject.Constants;

public class Charge : CharacterAction
{
    public Charge(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) { }

    ChargeAttackBuff chargeBuff;
    public override void Execute(CharacterManager target)
    {
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{User.username} is charging an attack")
        );
        if (LastAction is Charge) return;
        chargeBuff = new(User);
        chargeBuff.GetAdded();
    }
}
