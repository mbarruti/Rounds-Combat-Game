using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Charge")]
public class ChargeSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Charge(this);
    }
}
