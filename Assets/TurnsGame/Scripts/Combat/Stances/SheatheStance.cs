using static MyProject.Constants;

public class SheatheStance : Stance
{
    public SheatheStance(CharacterManager player) : base(player){}

    public override void EnterStance()
    {
        if (Applied) return;

        // TODO: make it recognize gender
        CombatUI.AddAnimation(
            CombatUI.Instance.WriteText($"{Player.username} sheathes their weapon"));
        ChargeBuffEffect chargeBuff = new();
        Player.AddEffect(chargeBuff);

        Player.OnPerformAction -= PrepareStanceExit;
        Player.OnPerformAction += PrepareStanceExit;

        Applied = true;
    }

    public override Stance Clone()
    {
        return new SheatheStance(Player);
    }

    protected override void ExitStance()
    {
        Player.ConsumeEffects(CHARGED_ATTACK);
        Player.stance = Player.chosenStance.Clone();
        Player.action.OnCompleted -= ExitStance;
    }

    protected override void PrepareStanceExit()
    {
        if (Player.action is Attack)
        {
            Player.action.OnCompleted -= ExitStance;
            Player.action.OnCompleted += ExitStance;
            Player.OnPerformAction -= PrepareStanceExit;
        }
    }
}

