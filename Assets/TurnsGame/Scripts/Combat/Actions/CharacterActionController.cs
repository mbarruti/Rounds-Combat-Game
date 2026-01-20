using System.Collections.Generic;

public class CharacterActionController
{
    public bool canUseAttack = true;
    public bool canUseBlock = true;
    public bool canUseCharge = true;
    public bool canUseTackle = false;
    public bool canUseParry = true;

    public List<CharacterActionSO> actionList = new();

    public void SetAvailableActions()
    {

    }
}
