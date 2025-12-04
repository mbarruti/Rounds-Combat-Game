using UnityEngine;
using MyProject;
using static MyProject.Constants;

public class Block : CharacterAction
{
    public Block(BlockSO charActionSO) : base(charActionSO){}

    Attack targetAttack;

    public override void Execute(CharacterManager player, CharacterManager target)
    {
        base.Execute(player, target);

        if (target.action is Attack auxAttack)
        {
            targetAttack = auxAttack;
            targetAttack.OnAttackHits -= OnTargetAttackHit;
            targetAttack.OnAttackHits += OnTargetAttackHit;
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey"));
        }
        Player.ConsumeEffects(ON_BLOCK);
        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry(Player.parryChance))
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!"));
            targetAttack.prowessBonus = -1.5f;
            targetAttack.meterDamageValue = 0;

            Player.nextAction = Player.attackSO.CreateAction();
            Player.nextAction.Execute(Player, target);
        }
        else
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText(Player.username + " blocks the incoming attack"));
            targetAttack.prowessBonus = -1;
        }
        // maybe targetAttack.OnAttackHits -= OnTargetAttackHit;
    }

    bool IsParry(float parryChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }
}
