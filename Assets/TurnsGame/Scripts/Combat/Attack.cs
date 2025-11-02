using UnityEngine;

public class Attack : Action
{
    int baseDamage = 20;
    int totalDamage;

    float meterDamage = 1f;

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        UI.AddAnimation(UI.Instance.WriteText(user.name + " attacks " + target.name));

        if (target.action is not Block)
        {
            totalDamage = baseDamage + 0;
            target.TakeDamage(totalDamage);
        }
        else
        {
            target.shieldMeter.LoseCharges(meterDamage);
            //UI.AddAnimation(target.shieldMeterUI.LoseChargeBars(meterDamage));
            Debug.Log("Number of charges of " + target.name + ": " + target.shieldMeter.GetAvailableCharges());
        }
        //user.RecoverShieldCharge();
    }
}
