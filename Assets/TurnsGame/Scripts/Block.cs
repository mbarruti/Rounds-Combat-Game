using UnityEngine;

public class Block : Action
{

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is Attack)
        {
            UI.Instance.WriteText(user.name + " blocks the incoming attack");
            user.LoseShieldCharge();
        }
        else UI.Instance.WriteText(user.name + " blocks nothing what a donkey");
    }
}
