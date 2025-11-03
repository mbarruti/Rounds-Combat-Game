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
            UI.AddAnimation(UI.Instance.WriteText(target.name + " blocks the incoming attack"));
            target.shieldMeter.LoseCharges(meterDamage);
            Debug.Log("Number of charges of " + target.name + ": " + target.shieldMeter.GetAvailableCharges());
        }
        //user.RecoverShieldCharge();
    }
}
