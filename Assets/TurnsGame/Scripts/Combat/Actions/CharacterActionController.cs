public class CharacterActionController
{
    public bool canUseAttack = true;
    public bool canUseBlock = true;
    public bool canUseCharge = true;
    public bool canUseTackle = false;
    public bool canUseParry = true;

    public void SetAvailableActions(CharacterAction lastAction)
    {
        switch (lastAction)
        {
            case Attack or Block or Parry or null:
                canUseAttack = true;
                canUseBlock = true;
                canUseCharge = true;
                canUseTackle = false;
                canUseParry = true;
                break;
            case Charge or Tackle:
                canUseAttack = true;
                canUseBlock = false;
                canUseCharge = true;
                canUseTackle = true;
                canUseParry = false;
                break;
        }
    }
}
