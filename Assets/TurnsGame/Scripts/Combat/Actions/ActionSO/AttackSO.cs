using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Attack")]
public class AttackSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        // return new Attack(this);
        return new Attack(this);
    }
}
