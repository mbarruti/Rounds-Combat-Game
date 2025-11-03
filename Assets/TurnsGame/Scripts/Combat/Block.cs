using UnityEngine;

public class Block : Action
{

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is Attack)
        {
            UI.AddAnimation(UI.Instance.WriteText(user.name + " blocks the incoming attack"));
            user.shieldMeter.LoseCharges(target.meterDamage);
            Debug.Log("Number of charges of " + user.name + ": " + user.shieldMeter.GetAvailableCharges());
        }
        else UI.AddAnimation(UI.Instance.WriteText(user.name + " blocks nothing what a donkey"));
    }
}
