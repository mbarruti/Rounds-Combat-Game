using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShieldMeter
{
    [SerializeField] int maxCharges;
    Stack<float> charges;
    [SerializeField] List<float> chargesList;

    public void Setup()
    {
        charges = new Stack<float>();
        maxCharges = 3;
        Refill();
    }

    public void Refill()
    {
        charges.Clear();
        for (int i = 0; i < maxCharges; i++)
        {
            charges.Push(1f);
        }
    }

    public void RecoverCharges()
    {
        if (GetAvailableCharges() == maxCharges)
            return;
        if (charges.Count() > 0 && IsHalf(charges.Peek()))
        {
            float charge = charges.Pop();
            charge = 1f;
            charges.Push(charge);
        }
        else
            charges.Push(0.5f);
        chargesList = new List<float>(charges);
    }

    public void LoseCharges(float shieldMeterDamage)
    {
        Stack<float> tempStack = new Stack<float>();

        while (GetAvailableCharges() > 0 && shieldMeterDamage > 0)
        {
            Debug.Log("Shield meter damage " + shieldMeterDamage);
            float chargeValue = charges.Pop();
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
        if (tempStack.Count() > 0) charges.Push(0.5f);
        chargesList = new List<float>(charges);
    }

    public int GetAvailableCharges()
    {
        return charges.Count(charge => !IsHalf(charge));
    }

    bool IsHalf(float value) => Mathf.Abs(value - 0.5f) < 0.0001f;

}
