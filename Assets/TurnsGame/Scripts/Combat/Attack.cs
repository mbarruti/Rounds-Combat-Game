using UnityEngine;

public class Attack : Action
{
    float totalDamage;

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is not Block)
        {
            if (AttackHits(user.accuracy))
            {
                totalDamage = (user.baseDamage + BonusDamage(user.baseDamage)) * user.prowess;
                target.TakeDamage(totalDamage);
            }
            else
            {
                UI.AddAnimation(UI.Instance.WriteText(user.name + " misses"));
            }
        }
        //user.RecoverShieldCharge();
    }

    int BonusDamage(float baseDamage)
    {
        int bonusDamage = 0;
        return bonusDamage;
    }

    bool AttackHits(float accuracy)
    {
        float randomValue = Random.Range(0, 11) / 10f;
        return randomValue <= accuracy;
    }
}
