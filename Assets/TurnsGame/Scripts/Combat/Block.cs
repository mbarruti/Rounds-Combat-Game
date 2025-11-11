using UnityEngine;

public class Block : CharacterAction
{
    public Block() {}

    public override void Execute(CharacterManager user, CharacterManager target)
    {
        if (target.action is Attack)
        {
            CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.username + " blocks the incoming attack"));

            if (IsParry(user.parryChance))
            {
                CombatUI.AddAnimation(CombatUI.Instance.WriteText($"{user.username} parries {target.username}!"));
                nextAction = new Attack();
                nextAction.Execute(user, target);
            }
            else user.TakeMeterDamage(target.meterDamage);
            
            //Debug.Log("Number of charges of " + user.name + ": " + user.shieldMeter.GetAvailableCharges());
        }
        else CombatUI.AddAnimation(CombatUI.Instance.WriteText(user.username + " blocks nothing what a donkey"));
    }

    bool IsParry(float parryChance)
    {
        float randomValue = Random.Range(0f, 1f);
        return parryChance >= randomValue;
    }
}
