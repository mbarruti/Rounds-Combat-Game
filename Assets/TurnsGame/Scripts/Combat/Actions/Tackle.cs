using System;
using System.Collections;
using MyProject;
using static MyProject.Constants;

public class Tackle : CharacterAction
{
    public Tackle(TackleSO charActionSO) : base(charActionSO){}

    Attack targetAttack;

    bool tackleSuccess = false;

    public override void OnExecute(CharacterManager player, CharacterManager target)
    {
        base.OnExecute(player, target);

        Anim.Sequence(
            Anim.Do(() =>
                CombatUI.Instance.WriteText($"{Player.username} tackles")
            )
        );

        // TODO: Check if this works when enemy does nothing
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
            Player.shieldMeter.RecoverCharges();
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, 1, ON_STANCE);
            Player.AddEffect(damageBuff);
            damageBuff.Apply(Player, null);
        }
        else
        {
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText($"{Player.username} fails tackle attempt")
                )
            );
            Player.action = new(null);
            Player.stance = Player.chosenStance.Clone();
            Player.ConsumeEffects(ON_STANCE);
        }
        Player.ConsumeEffects(TACKLE);
        CompleteAction();

        targetAttack.OnCompleted -= OnTargetActionCompleted; // MAYBE NOT NECESSARY
    }
}
