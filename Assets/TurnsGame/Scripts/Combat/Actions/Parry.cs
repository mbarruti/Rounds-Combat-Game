using UnityEngine;
using MyProject;
using static MyProject.Constants;

public class Parry : CharacterAction
{
    public Parry(ParrySO charActionSO) : base(charActionSO){}

    bool parrySuccess = false;

    Attack targetAttack;

    public override void Execute(CharacterManager player, CharacterManager target)
    {
        base.Execute(player, target);

        if (Target.action != null)
        {
            Target.action.OnCompleted -= OnTargetActionCompleted;
            Target.action.OnCompleted += OnTargetActionCompleted;

            if (Target.action is Attack auxAttack)
            {
                targetAttack = auxAttack;
                targetAttack.OnAttackHits -= OnTargetAttackHit;
                targetAttack.OnAttackHits += OnTargetAttackHit;
            }
        }
        else OnTargetActionCompleted();

        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry())
        {
            parrySuccess = true;
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!"));

            targetAttack.prowessBonus = -1.5f;
            Player.nextAction = Player.attackSO.CreateAction();
            Player.nextAction.Execute(Player, target);
        }
        // maybe targetAttack.OnAttackHits -= OnTargetAttackHit;
    }

    bool IsParry(float parryChance = FORCED_PARRY_CHANCE)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }

    void OnTargetActionCompleted()
    {
        if (parrySuccess == false)
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} fails parry attempt"));

            Player.shieldMeter.LoseCharges(PARRY_METER_COST);
        }
    }
}
