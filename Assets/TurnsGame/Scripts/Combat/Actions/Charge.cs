using NUnit.Framework;
using MyProject;
using static MyProject.Constants;
using UnityEngine;

public class Charge : CharacterAction
{
    public Charge(ChargeSO charActionSO) : base(charActionSO){}

    public override void OnExecute(CharacterManager player, CharacterManager target)
    {
        base.OnExecute(player, target);

        // TODO: think of a way to get the same logic as the if without having to check LastAction
        if (/* Player.lastAction is not (Charge or Tackle) */Player.stance is not ChargeStance)
        {
            Player.stance = new ChargeStance(Player);
            Player.stance.EnterStance();
            //chargeBuff.Apply(Player, target);
        }
        else
        {
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText($"{Player.username} keeps charging")
                )
            );
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, SINGLE_USE, ON_STANCE);
            Player.AddEffect(damageBuff);
            //damageBuff.Apply(Player, target);
        }
        Player.ApplyEffects(ON_STANCE);
        CompleteAction();
    }
}
