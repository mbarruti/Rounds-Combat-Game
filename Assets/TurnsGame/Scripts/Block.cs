using UnityEngine;

public class Block : Action
{

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is Attack)
        {
            UI.AddAnimation(UI.Instance.TypeTextCoroutine(user.name + " blocks the incoming attack"));
            user.LoseShieldCharge();
        }
        else UI.AddAnimation(UI.Instance.TypeTextCoroutine(user.name + " blocks nothing what a donkey"));
    }
}
