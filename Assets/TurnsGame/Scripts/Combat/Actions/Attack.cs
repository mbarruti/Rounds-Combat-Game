using UnityEngine;
using System;
using MyProject;
using static MyProject.Constants;
using UnityEngine.TextCore.Text;

public class Attack : CharacterAction
{
    public Attack(CharacterManager user, CharacterAction lastAction) : base(user, lastAction)
    {
        Lead = LOW;
        CanRecoverMeter = true;
    }

    float totalBaseDamage = 0;
    public float prowessBonus = 0;
    float totalDamage = 0;

    public event Action<CharacterManager> OnAttackHits;

    public override void Execute(CharacterManager target)
    {
        Player.ApplyEffects(ATTACK);

        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText(Player.username + " attacks " + target.username));

        // TODO: think of a way to Invoke event one time inside the for
        // taking AttackHits into account once for Parry
        OnAttackHits?.Invoke(Player);

        for (int hitNumber = 0; hitNumber < Player.numHits; hitNumber++)
        {
            if (AttackHits(Player.accuracy))
            {
                totalBaseDamage = BonusBaseDamage();
                totalDamage = (totalBaseDamage + BonusDamage()) * ProwessValue(Player.prowess);
                if (!Mathf.Approximately(totalDamage, 0)) target.TakeDamage(totalDamage);
                if (target.action is Block) target.TakeMeterDamage(Player.meterDamage);
            }
            else
            {
                CombatUI.AddAnimation(CombatUI.Instance.WriteText(Player.username + " misses"));
            }
        }

        //user.RecoverShieldCharge();
        Player.ConsumeEffects(ATTACK);
        Player.ConsumeEffects(CHARGED_ATTACK);
        CompleteAction();
    }

    float BonusBaseDamage()
    {
        float bonus = Player.baseDamage + Player.activeBuffs.BaseDamage;
        return bonus;
    }

    float BonusDamage()
    {
        float bonusDamage = Player.baseDamage * Player.activeBuffs.BonusDamage;
        return bonusDamage;
    }

    float ProwessValue(float prowess)
    {
        prowess += Player.activeBuffs.Prowess;
        if (prowess < 0 || Mathf.Approximately(prowess, 0)) prowess = 0;
        else if (prowess > 1) prowess = 1;

        prowess += prowessBonus;
        if (prowess < 0 || Mathf.Approximately(prowess, 0)) prowess = 0;
        else if (prowess > 1) prowess = 1;
        return prowess;
    }

    bool AttackHits(float accuracy)
    {
        accuracy += Player.activeBuffs.Accuracy;
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        return randomValue <= accuracy;
    }
}
