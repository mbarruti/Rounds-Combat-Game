using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Tackle")]
public class TackleSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Tackle(this);
    }

    protected override bool OnCanCreate(CharacterManager player)
    {
        if (player.lastAction is not (Charge or Tackle)) return false;
        return true;
    }
}
