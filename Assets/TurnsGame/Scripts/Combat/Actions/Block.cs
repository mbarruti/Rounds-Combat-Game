using UnityEngine;
using MyProject;
using static MyProject.Constants;

public class Block : CharacterAction
{
    public Block(BlockSO charActionSO) : base(charActionSO){}

    Attack targetAttack;

    public override void OnExecute(CharacterManager player, CharacterManager target)
    {
        base.OnExecute(player, target);

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;
            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
        }
        else
        {
            // CombatUI.AddAnimation(
            //     CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey"));
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey")
                )
            );
        }
        Player.ConsumeEffects(ON_BLOCK);
        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry(Player.parryChance))
        {
            // CombatUI.AddAnimation(
            //     CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!"));
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
            // CombatUI.AddAnimation(
            //     CombatUI.Instance.WriteText(Player.username + " blocks the incoming attack"));
            Anim.Sequence(
                Anim.Do(() =>
                    CombatUI.Instance.WriteText(Player.username + " blocks the incoming attack")
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
}
