using MyProject;
using UnityEngine;
using static MyProject.Constants;

public class Attack : CharacterAction
{
    public Attack(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}

    float totalBaseDamage = 0;
    float totalDamage = 0;
    public float prowessBonus = 0;

    public override void Execute(CharacterManager target)
    {
        Player.ApplyEffects(ATTACK);
        Player.ApplyEffects(CHARGED_ATTACK);

        totalBaseDamage = BonusBaseDamage();
        totalDamage = (totalBaseDamage + BonusDamage()) * ProwessValue(Player.prowess);
        if (totalDamage <= 0) return;

        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText(Player.username + " attacks " + target.username));

        if (target.action is not Block)
        {
            for (int hitNumber = 0; hitNumber < Player.numHits; hitNumber++)
            {
                if (AttackHits(Player.accuracy))
                {
                    target.TakeDamage(totalDamage);
                }
                else
                {
                    CombatUI.AddAnimation(CombatUI.Instance.WriteText(Player.username + " misses"));
                }
            }
        }
        //user.RecoverShieldCharge();
    }

    float BonusDamage()
    {
        float bonusDamage = Player.baseDamage * Player.activeBuffs.BonusDamage;
        return bonusDamage;
    }

    float BonusBaseDamage()
    {
        float bonus = Player.baseDamage + Player.activeBuffs.BaseDamage;
        return bonus;
    }

    float ProwessValue(float prowess)
    {
        prowess += Player.activeBuffs.Prowess + prowessBonus;
        if (prowess < 0) prowess = 0;
        else if (prowess > 1) prowess = 1;
        return prowess;
    }

    bool AttackHits(float accuracy)
    {
        accuracy += Player.activeBuffs.Accuracy;
        float randomValue = Random.Range(0f, 1f);
        return randomValue <= accuracy;
    }
}
