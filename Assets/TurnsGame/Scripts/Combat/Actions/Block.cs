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

        Anim.Sequence(
            Anim.Do(() => Player.rigController.IdleAnimation("BlockIdle"))
        );

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;
            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
        }
        else
        {
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey")
                ),
                Anim.Do(() => Player.rigController.IdleAnimation("DefaultIdle"))
            );
        }
        Player.ConsumeEffects(ON_BLOCK);
        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry(Player.parryChance))
        {
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!")
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
            Anim.Sequence(
                Anim.Parallel(
                    Anim.Do(() =>
                        CombatUI.Instance.WriteText(
                            $"{Player.username} blocks the incoming attack", waitTime: 0)
                    ),
                    Anim.Do(() => Player.rigController.ActionAnimation("Block"))
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
