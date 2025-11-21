using NUnit.Framework;
using UnityEngine;
using static MyProject.Constants;

public class Charge : CharacterAction
{
    public Charge(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}
    public override void Execute(CharacterManager target)
    {
        if (LastAction is not (Charge or Tackle))
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} is charging an attack"));
            ChargeBuffEffect chargeBuff = new();
            Player.AddEffect(chargeBuff);
            Player.ApplyEffects(CHARGED_ATTACK);
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} keeps charging"));
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, SINGLE_USE, CHARGED_ATTACK);
            Player.AddEffect(damageBuff);
            Player.ApplyEffects(CHARGED_ATTACK);
        }
        CompleteAction();
    }
}
