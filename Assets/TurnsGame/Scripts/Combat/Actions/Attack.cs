using UnityEngine;
using System;
using MyProject;
using static MyProject.Constants;
using UnityEngine.TextCore.Text;

public class Attack : CharacterAction
{
    public Attack(AttackSO charActionSO) : base(charActionSO){}

    float totalBaseDamage = 0;
    public float prowessBonus = 0;
    float totalDamage = 0;

    public event Action<CharacterManager> OnAttackHits;

    public override void Execute(CharacterManager player, CharacterManager target)
    {
        Player = player;
        Player.ApplyEffects(ON_ATTACK);

        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText(Player.username + " attacks " + target.username));

        // TODO: think of a way to Invoke event one time inside the for
        // taking AttackHits into account once for Parry
        OnAttackHits?.Invoke(Player);

        totalBaseDamage = BonusBaseDamage();
        totalDamage = (totalBaseDamage + BonusDamage()) * ProwessValue(Player.prowess);

        int successfulHits = 0;
        for (int hitNumber = 0; hitNumber < Player.numHits; hitNumber++)
        {
            if (AttackHits(Player.accuracy))
            {
                successfulHits += 1;
                if (!Mathf.Approximately(totalDamage, 0)) target.TakeDamage(totalDamage);
            }
            else
            {
                CombatUI.AddAnimation(CombatUI.Instance.WriteText(Player.username + " misses"));
            }
        }
        if (successfulHits > 0) target.TakeMeterDamage(Player.meterDamage);

        // for (int i = 0; i < successfulHits; i++)
        // {
        //     if (!Mathf.Approximately(totalDamage, 0)) target.TakeDamage(totalDamage);
        // }

        //user.RecoverShieldCharge();
        Player.ConsumeEffects(ON_ATTACK);
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
        prowess += prowessBonus;
        if (prowess < 0 || Mathf.Approximately(prowess, 0)) prowess = 0;
        else if (prowess > 1) prowess = 1;

        prowess += Player.activeBuffs.Prowess;
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
