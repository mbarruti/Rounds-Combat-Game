using UnityEngine;

public class Block : CharacterAction
{
    public Block() {}

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is Attack)
        {
            CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.name + " blocks the incoming attack"));
            user.TakeMeterDamage(target.meterDamage);
            Debug.Log("Number of charges of " + user.name + ": " + user.shieldMeter.GetAvailableCharges());
        }
        else CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.name + " blocks nothing what a donkey"));
    }
}
