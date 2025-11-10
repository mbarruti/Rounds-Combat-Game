using UnityEngine;

public class Attack : CharacterAction
{
    float totalDamage = 0;
    public float prowessBonus = 0;

    public Attack() {}

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        totalDamage = (user.baseDamage + BonusDamage(user.baseDamage)) * ProwessValue(user.prowess);
        if (totalDamage <= 0) return;
        CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.username + " attacks " + target.username));
        if (target.action is not Block)
        {
            for (int hitNumber = 0; hitNumber < user.numHits; hitNumber++)
            {
                if (AttackHits(user.accuracy))
                {
                    target.TakeDamage(totalDamage);
                }
                else
                {
                    CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.username + " misses"));
                }
            }
        }
        //user.RecoverShieldCharge();
    }

    float BonusDamage(float baseDamage)
    {
        float bonusDamage = 0;
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
