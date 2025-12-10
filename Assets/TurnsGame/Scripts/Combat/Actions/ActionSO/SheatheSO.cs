
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Sheathe")]
public class SheatheSO : CharacterActionSO
{
    public override CharacterAction CreateAction()
    {
        return new Sheathe(this);
    }
}
