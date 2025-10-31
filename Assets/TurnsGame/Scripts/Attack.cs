using System.Xml.Linq;
using UnityEngine;

public class Attack : Action
{
    int baseDamage = 20;
    int totalDamage;

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        UI.AddAnimation(UI.Instance.WriteText(user.name + " attacks " + target.name));

        if (target.action is not Block)
        {
            totalDamage = baseDamage + 0;
            target.TakeDamage(totalDamage);
        }
        //user.RecoverShieldCharge();
    }
}
