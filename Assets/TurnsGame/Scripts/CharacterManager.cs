using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterManager : MonoBehaviour
{

    public int maxHP;
    public int currentHP;

    public int maxShieldCharges;
    public int currentShieldCharges;

    public Action action;

    public void Setup(bool isPlayer)
    {
        if (isPlayer)
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().playerMaterial;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().enemyMaterial;
        }
    }

    public void PerformAction(CharacterManager target)
    {
        action?.Execute(this, target);
        if (action is not Block) RecoverShieldCharge();
    }

    public void TakeDamage(int damage)
    {
        UI.Instance.WriteText(this.name + " loses " + damage + " health points");

        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        UI.Instance.WriteText(this.name + " has " + currentHP + " health points left");
    }

    private void RecoverShieldCharge()
    {
        if (currentShieldCharges < maxShieldCharges) currentShieldCharges += 1;
    }

    public void LoseShieldCharge()
    {
        if (currentShieldCharges > 0) currentShieldCharges -= 1;
    }
}
