using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Charge")]
public class ChargeSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Charge(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (player.state == RECOVER ||
            player.state == WAIT) return false;
        return true;
    }
}
