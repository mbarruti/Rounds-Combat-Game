using UnityEngine;
using static MyProject.Constants;

public class Charge : CharacterAction
{
    public Charge(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}
    public override void Execute(CharacterManager target)
    {
        if (LastAction is not Charge)
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} is charging an attack"));
            ChargeBuffEffect chargeBuff = new();
            Debug.Log(chargeBuff.GetType());
            chargeBuff.GetAdded(Player, target);
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} keeps charging"));
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, 1, CHARGED_ATTACK);
            Debug.Log(damageBuff.GetType());
            damageBuff.GetAdded(Player, target);
        }
    }
}
