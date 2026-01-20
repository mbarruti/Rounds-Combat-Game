using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Parry")]
public class ParrySO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Parry(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (player.isDual ||
            player.stance is ChargeStance ||
            player.state == RECOVER ||
            player.state == WAIT) return false;
        return true;
    }
}
