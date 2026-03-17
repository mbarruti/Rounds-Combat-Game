using UnityEngine;
using MyProject;
using static MyProject.Constants;
using Cysharp.Threading.Tasks;

public class Block : CharacterAction
{
    public Block(BlockSO charActionSO) : base(charActionSO){}

    Attack targetAttack;

    public override void OnExecute(CharacterManager player, CharacterManager target)
    {
        base.OnExecute(player, target);

        Act.Sequence(
            Act.Do(() => Player.rigController.IdleAnimation("BlockIdle"))
        );

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;
            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
        }
        else
        {
            Act.Sequence(
                Act.Do(() =>
                    CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey")
                )/* ,
                Anim.Do(() => Player.rigController.IdleAnimation("DefaultIdle")) */
            );
        }
        Player.ConsumeEffects(ON_BLOCK);
        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry(Player.parryChance))
        {
            Act.Sequence(
                Act.Parallel(
/*                     Act.Do(() =>
                        CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!")
                    ), */
                    Act.Do(() => Player.rigController.ActionAnimation("Parry", trigger: 0.3f)),
                    Act.Do(() => Target.rigController.ActionAnimation("Counter", trigger: 0.5f))
                )
            );

            targetAttack.prowessBonus = PARRY_PROWESS_LOSS;
            targetAttack.meterDamageValue = 0;
            Target.state = RECOVER;

            Player.nextAction = Player.attackSO.CreateAction();
            Player.nextAction.Execute(Player, target);
        }
        else
        {
            Act.Sequence(
                Act.Parallel(
                    Act.Do(() =>
                        CombatUI.Instance.WriteText(
                            $"{Player.username} blocks the incoming attack", waitTime: 0)
                    ),
                    Act.Do(() => Player.rigController.ActionAnimation("Block"))
                )
            );

            targetAttack.prowessBonus = BLOCK_PROWESS_LOSS;
        }
        // maybe targetAttack.OnAttackHits -= OnTargetAttackHit;
    }

    bool IsParry(float parryChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }

    // private async UniTask BlockAnimation()
    // {
    //     Player.isPerformingAction = true;
    //     Player.animator.CrossFadeInFixedTime("Block", 0.2f);

    //     while (Player.isPerformingAction)
    //     {
    //         await UniTask.Yield(PlayerLoopTiming.Update);
    //     }
    // }
}
