using UnityEngine;
using UnityEngine.TextCore.Text;
using static MyProject.Constants;

public class ChargeStance : Stance
{
    public ChargeStance(CharacterManager player) : base(player){}

    public override void EnterStance()
    {
        if (Applied) return;

        Anim.Sequence(
            Anim.Do(() =>
                CombatUI.Instance.WriteText($"{Player.username} is charging an attack")
            )
        );

        ChargeBuffEffect chargeBuff = new();
        Player.AddEffect(chargeBuff);

        Player.OnPerformAction -= PrepareStanceExit;
        Player.OnPerformAction += PrepareStanceExit;

        Applied = true;
    }

    public override Stance Clone()
    {
        return new ChargeStance(Player);
    }

    protected override void ExitStance()
    {
        //Debug.Log(Player.activeBuffs.BonusDamage);
        // if (Player.effects.TryGetValue(ON_STANCE, out var list))
        // {
        //     Player.ConsumeEffects(ON_STANCE);
        //     list.Clear();
        // }
        Player.ConsumeEffects(ON_STANCE);
        //Debug.Log(Player.activeBuffs.BonusDamage);
        Player.stance = Player.chosenStance.Clone();
        Player.action.OnCompleted -= ExitStance;
        //Player.OnPerformAction -= PrepareStanceExit;
    }

    protected override void PrepareStanceExit()
    {
        if (Player.action is Attack || Player.action.Type == ActionType.NoType)
        {
            Player.action.OnCompleted -= ExitStance;
            Player.action.OnCompleted += ExitStance;
            Player.OnPerformAction -= PrepareStanceExit;
        }
    }
}

