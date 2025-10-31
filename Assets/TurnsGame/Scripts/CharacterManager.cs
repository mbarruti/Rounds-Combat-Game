using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public TextMeshProUGUI healthText;

    public int maxShieldCharges;
    public Stack<float> shieldCharges;
    public List<float> chargesList;
    //public int currentShieldCharges;

    public Action action;

    public void Setup(bool isPlayer)
    {
        shieldCharges = new Stack<float>();
        RefillShieldMeter();

        if (isPlayer)
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().playerMaterial;
            healthText = UI.Instance.playerHPText;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = CombatManager.GetInstance().enemyMaterial;
            healthText = UI.Instance.enemyHPText;
        }
        healthText.text = $"{currentHP}/{maxHP}";
    }

    public void PerformAction(CharacterManager target)
    {
        action?.Execute(this, target);
        if (action is not Block) RecoverShieldCharge();
    }

    public void TakeDamage(int damage)
    {
        UI.AddAnimation(UI.Instance.WriteText(name + " loses " + damage + " health points"));

        if (currentHP - damage < 0) currentHP = 0;
        else currentHP -= damage;

        UI.AddAnimation(UI.Instance.UpdateHPText(this));
    }

    public void RefillShieldMeter()
    {
        for (int i = 0; i < maxShieldCharges; i++)
        {
            shieldCharges.Push(1f);
        }
    }

    private void RecoverShieldCharge()
    {
        if (GetAvailableCharges() == maxShieldCharges)
            return;
        if (shieldCharges.Count() > 0 && IsHalf(shieldCharges.Peek()))
        {
            float charge = shieldCharges.Pop();
            charge = 1f;
            shieldCharges.Push(charge);
        }
        else
            shieldCharges.Push(0.5f);
        //if (currentShieldCharges < maxShieldCharges) currentShieldCharges += 1;
        chargesList = new List<float>(shieldCharges);
        Debug.Log("Number of charges of " + this.name + ": " + GetAvailableCharges());
    }

    public void LoseShieldCharges(float shieldMeterDamage)
    {
        Stack<float> tempStack = new Stack<float>();

        while (GetAvailableCharges() > 0 && shieldMeterDamage > 0)
        {
            Debug.Log("daño a shield " + shieldMeterDamage);
            float chargeValue = shieldCharges.Pop();
            if (IsHalf(chargeValue)) // If it's 0.5f
            {
                tempStack.Push(chargeValue);
            }
            else // If it's 1f
            {
                float damageLeft = shieldMeterDamage - chargeValue;
                chargeValue -= shieldMeterDamage;
                if (IsHalf(chargeValue))
                    tempStack.Push(chargeValue);
                shieldMeterDamage = damageLeft;
            }
        }
        if (tempStack.Count() > 0) shieldCharges.Push(0.5f);
        //if (currentShieldCharges > 0) currentShieldCharges -= 1;
        chargesList = new List<float>(shieldCharges);
        Debug.Log("Number of charges of " + this.name + ": " + GetAvailableCharges());
    }

    public int GetAvailableCharges()
    {
        return shieldCharges.Count(charge => !IsHalf(charge));
    }

    //bool IsChargeRecovering(float charge)
    //{
    //    const float epsilon = 0.0001f;
    //    return charge < 0.5f + epsilon;
    //}

    bool IsHalf(float value) => Mathf.Abs(value - 0.5f) < 0.0001f;

    public bool IsDead()
    {
        if (currentHP <= 0) return true;
        return false;
    }
}
