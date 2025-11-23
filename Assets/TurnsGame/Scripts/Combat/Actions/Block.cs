using UnityEngine;
using MyProject;
using static MyProject.Constants;

public class Block : CharacterAction
{
    public Block(CharacterManager user, CharacterAction lastAction) : base(user, lastAction)
    {
        Lead = MEDIUM;
    }

    Attack targetAttack;

    public override void Execute(CharacterManager target)
    {
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
        Player.ConsumeEffects(BLOCK);
        CompleteAction();
    }

    void OnTargetAttackHit(CharacterManager target)
    {
        if (IsParry(Player.parryChance))
        {
            CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!"));

            NextAction = new Attack(Player, this);
            NextAction.Execute(target);
        }
        else CombatUI.AddAnimation(
                CombatUI.Instance.WriteText(Player.username + " blocks the incoming attack"));
        targetAttack.prowessBonus = -1;
        // maybe targetAttack.OnAttackHits -= OnTargetAttackHit;
    }

    bool IsParry(float parryChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }
}
