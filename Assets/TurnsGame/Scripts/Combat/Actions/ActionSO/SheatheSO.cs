using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Sheathe")]
public class SheatheSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Sheathe(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (!player.isDual ||
            player.stance is SheatheStance ||
            player.state == RECOVER ||
            player.state == WAIT) return false;
        return true;
    }
}
