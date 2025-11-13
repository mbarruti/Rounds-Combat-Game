using UnityEngine;
using static MyProject.Constants;

public class Charge : CharacterAction
{
    public Charge(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}

    DamageBuff chargeBuff = new(CHARGE_BUFF);
    public override void Execute(CharacterManager target)
    {
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{Player.username} is charging an attack"));
        if (LastAction is not Charge)
        {
            chargeBuff.GetAdded(Player, target);
        }
    }
}
