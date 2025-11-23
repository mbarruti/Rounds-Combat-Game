using System;
using System.Collections;
using UnityEngine;
using MyProject;
using static MyProject.Constants;

public class Tackle : CharacterAction
{
    public Tackle(CharacterManager user, CharacterAction lastAction) : base(user, lastAction)
    {
        Lead = HIGH;
    }

    Attack targetAttack;

    bool tackleSuccess = false;

    public override void Execute(CharacterManager target)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Player.username} tackles"));

        target.action.OnCompleted -= OnTargetActionCompleted;
        target.action.OnCompleted += OnTargetActionCompleted;

        Player.shieldMeter.LoseCharges(HALF_CHARGE);

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;

            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
        }
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        tackleSuccess = true;
        DmgReductionEffect reduction = new(TACKLE_DMG_REDUCTION, SINGLE_USE, TACKLE);
        Player.AddEffect(reduction);
        Player.ApplyEffects(TACKLE);

        targetAttack.OnAttackHits -= OnTargetAttackHit; // MAYBE NOT NECESSARY
    }

    void OnTargetActionCompleted()
    {
        if (tackleSuccess)
        {
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, 1, CHARGED_ATTACK);
            Player.AddEffect(damageBuff);
            damageBuff.Apply(Player, null);
            Player.shieldMeter.RecoverCharges();
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} fails tackle attempt"));
            Player.action = null;
            Player.ConsumeEffects(CHARGED_ATTACK);
        }
        Player.ConsumeEffects(TACKLE);
        CompleteAction();

        targetAttack.OnCompleted -= OnTargetActionCompleted; // MAYBE NOT NECESSARY
    }
}
