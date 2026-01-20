using UnityEngine;
using static MyProject.Constants;

[CreateAssetMenu(menuName = "Character Actions/Attack")]
public class AttackSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Attack(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (player.state == RECOVER || player.state == WAIT) return false;
        return true;
    }
}
