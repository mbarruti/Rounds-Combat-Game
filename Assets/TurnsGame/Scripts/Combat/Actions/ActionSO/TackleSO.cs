using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Tackle")]
public class TackleSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Tackle(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (!player.isDual ||
            player.stance is not ChargeStance ||
            player.state == RECOVER ||
            player.state == WAIT) return false;
        return true;
    }
}
