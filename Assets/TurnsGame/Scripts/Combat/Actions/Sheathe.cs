using static MyProject.Constants;

public class Sheathe : CharacterAction
{
    public Sheathe(SheatheSO charActionSO) : base(charActionSO){}

    public override void OnExecute(CharacterManager player, CharacterManager target)
    {
        base.OnExecute(player, target);

        if (/* Player.lastAction is not Sheathe */Player.stance is not SheatheStance)
        {
            Player.stance = new SheatheStance(Player);
            Player.stance.EnterStance();
        }
        else CombatUI.AddAnimation(
                CombatUI.Instance.WriteText($"{Player.username} keeps the weapon sheathed"));

        Player.ApplyEffects(CHARGED_ATTACK);
        CompleteAction();
    }
}
