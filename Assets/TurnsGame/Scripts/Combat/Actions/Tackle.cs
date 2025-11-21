using System.Collections;
using UnityEngine;
using static MyProject.Constants;

public class Tackle : CharacterAction
{
    Attack targetAttack;
    public Tackle(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}
    public override void Execute(CharacterManager target)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Player.username} tackles"));

        Player.shieldMeter.LoseCharges(HALF_CHARGE);

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;
            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
            targetAttack.OnCompleted -= OnTargetAttackCompleted;
            targetAttack.OnCompleted += OnTargetAttackCompleted;
        }
        else
        {
            Player.action = null;
            Player.ConsumeEffects(CHARGED_ATTACK);
        }

        Player.ConsumeEffects(TACKLE);
        CompleteAction();
    }

    void OnTargetAttackHit()
    {
        DmgReductionEffect reduction = new(TACKLE_DMG_REDUCTION, SINGLE_USE, TACKLE);
        Player.AddEffect(reduction);
        Player.ApplyEffects(TACKLE);
        targetAttack.OnAttackHits -= OnTargetAttackHit; // MAYBE NOT NECESSARY
    }

    void OnTargetAttackCompleted()
    {
        DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, 1, CHARGED_ATTACK);
        Player.AddEffect(damageBuff);
        Player.shieldMeter.RecoverCharges();
        targetAttack.OnCompleted -= OnTargetAttackCompleted; // MAYBE NOT NECESSARY
    }
}
