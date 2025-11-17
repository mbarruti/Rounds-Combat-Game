using UnityEngine;
using static MyProject.Constants;

public class Tackle : CharacterAction
{
    public Tackle(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}
    public override void Execute(CharacterManager target)
    {
        CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Player.username} tackles"));

        DmgReductionEffect reduction = new(TACKLE_DMG_REDUCTION, 1, TACKLE);
        reduction.GetAdded(Player, null);
        Player.ApplyEffects(TACKLE);

        if (target.action is Attack)
        {
            DamageBuffEffect damageBuff = new(CHARGE_DAMAGE_BUFF, 1, CHARGED_ATTACK);
            damageBuff.GetAdded(Player, target);
        }
        else
        {
            Player.action = null;
            Player.effects[CHARGED_ATTACK].Clear();
        }
    }
}
