using NUnit.Framework;
using UnityEngine;
using MyProject;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Charge")]
public class ChargeSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Charge(this);
    }
}

public class Charge : CharacterAction
{
    public Charge(ChargeSO chargeSO)
    {
        DataSO = chargeSO;
    }

    public override void Execute(CharacterManager player, CharacterManager target)
    {
        Player = player;

        // TODO: think of a way to get the same logic as the if without having to check LastAction
        if (LastAction is not (Charge or Tackle))
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} is charging an attack"));
            ChargeBuffEffect chargeBuff = new();
            Player.AddEffect(chargeBuff);
            //chargeBuff.Apply(Player, target);
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} keeps charging"));
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, SINGLE_USE, CHARGED_ATTACK);
            Player.AddEffect(damageBuff);
            //damageBuff.Apply(Player, target);
        }
        Player.ApplyEffects(CHARGED_ATTACK);
        CompleteAction();
    }
}
