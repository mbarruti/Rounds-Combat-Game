using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Tackle")]
public class TackleSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Tackle(this);
    }

    public override bool CanCreateAction(CharacterManager player)
    {
        if (player.lastAction is not (Charge or Tackle)) return false;
        return true;
    }
}
