using UnityEngine;

public class Block : CharacterAction
{
    public Block(CharacterManager user, CharacterAction lastAction) : base(user, lastAction) {}

    public override void Execute(CharacterManager target)
    {
        if (target.action is Attack)
        {
            CombatUI.AddAnimation(CombatUI.Instance.WriteText(Player.username + " blocks the incoming attack"));

            if (IsParry(Player.parryChance))
            {
                CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{Player.username} parries {target.username}!"));
                NextAction = new Attack(Player, this);
                NextAction.Execute(target);
            }
            else Player.TakeMeterDamage(target.meterDamage);

            //Debug.Log("Number of charges of " + user.name + ": " + user.shieldMeter.GetAvailableCharges());
        }
        else CombatUI.AddAnimation(CombatUI.Instance.WriteText(Player.username + " blocks nothing what a donkey"));
    }

    bool IsParry(float parryChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }
}
