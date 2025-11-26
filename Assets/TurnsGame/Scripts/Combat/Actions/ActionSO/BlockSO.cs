using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Block")]
public class BlockSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Block(this);
    }
}
