using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Tackle")]
public class TackleSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Tackle(this);
    }
}
