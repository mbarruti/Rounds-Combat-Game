using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Parry")]
public class ParrySO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Parry(this);
    }
}
