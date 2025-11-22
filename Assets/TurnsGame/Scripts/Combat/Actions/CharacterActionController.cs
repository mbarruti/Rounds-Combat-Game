public class CharacterActionController
{
    public bool canUseAttack = true;
    public bool canUseBlock = true;
    public bool canUseCharge = true;
    public bool canUseTackle = false;

    public void SetAvailableActions(CharacterAction lastAction)
    {
        switch (lastAction)
        {
            case Attack or Block or null:
                canUseAttack = true;
                canUseBlock = true;
                canUseCharge = true;
                canUseTackle = false;
                break;
            case Charge or Tackle:
                canUseAttack = true;
                canUseBlock = false;
                canUseCharge = true;
                canUseTackle = true;
                break;
        }
    }
}
