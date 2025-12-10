public class DefaultStance : Stance
{
    public DefaultStance(CharacterManager player) : base(player){}

    public override Stance Clone()
    {
        return new DefaultStance(Player);
    }

    public override void EnterStance()
    {
    }

    protected override void ExitStance()
    {
    }

    protected override void PrepareStanceExit()
    {
    }
}

