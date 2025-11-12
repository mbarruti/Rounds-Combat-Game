using UnityEngine;
using static MyProject.Constants;

public class Attack : CharacterAction
{
    float totalDamage = 0;
    public float prowessBonus = 0;

    public Attack(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) { }

    public override void Execute(CharacterManager target)
    {
        totalDamage = (User.baseDamage + BonusDamage()) * ProwessValue(User.prowess);
        if (totalDamage <= 0) return;
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText(User.username + " attacks " + target.username)
        );
        if (target.action is not Block)
        {
            for (int hitNumber = 0; hitNumber < User.numHits; hitNumber++)
            {
                if (AttackHits(User.accuracy))
                {
                    target.TakeDamage(totalDamage);
                }
                else
                {
                    CombatUI.AddAnimation(CombatUI.Instance.WriteText(User.username + " misses"));
                }
            }
        }
        //user.RecoverShieldCharge();
    }

    float BonusDamage()
    {
        float bonusDamage = User.baseDamage * (float)User.activeBuffs[DAMAGE].Use();
        return bonusDamage;
    }

    float ProwessValue(float prowess)
    {
        float totalProwess = prowess + prowessBonus;
        if (totalProwess < 0) totalProwess = 0;
        else if (totalProwess > 1) totalProwess = 1;
        return totalProwess;
    }

    bool AttackHits(float accuracy)
    {
        float randomValue = Random.Range(0f, 1f);
        return randomValue <= accuracy;
    }
}
