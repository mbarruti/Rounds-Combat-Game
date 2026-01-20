using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Block")]
public class BlockSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Block(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if ((player.isDual && player.stance is ChargeStance) ||
            player.stance is SheatheStance ||
            player.state == RECOVER ||
            player.state == WAIT) return false;
        return true;
    }
}
