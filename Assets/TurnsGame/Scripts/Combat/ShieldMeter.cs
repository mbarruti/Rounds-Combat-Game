using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShieldMeter
{
    [SerializeField] int maxCharges;
    Stack<float> charges;

    public event Action<List<float>> chargesChangedEvent;

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
            charges.Push(Constants.FULL_CHARGE);
        }
    }

    public void RecoverCharges()
    {
        if (GetAvailableCharges() == maxCharges)
            return;
        if (charges.Count > 0 && IsHalf(charges.Peek()))
        {
            float charge = charges.Pop();
            charge = Constants.FULL_CHARGE;
            charges.Push(charge);
        }
        else
            charges.Push(Constants.HALF_CHARGE);
        chargesChangedEvent?.Invoke(GetChargesCopy());
    }

    public void LoseCharges(float shieldMeterDamage)
    {
        Stack<float> tempStack = new Stack<float>();

        while (GetAvailableCharges() > 0 && shieldMeterDamage > 0)
        {
            Debug.Log("Shield meter damage left: " + shieldMeterDamage);
            float chargeValue = charges.Pop();
            if (IsHalf(chargeValue)) // If charge is recovering
            {
                tempStack.Push(chargeValue);
            }
            else // If charge is available
            {
                float damageLeft = shieldMeterDamage - chargeValue;
                chargeValue -= shieldMeterDamage;
                if (IsHalf(chargeValue))
                    tempStack.Push(chargeValue);
                shieldMeterDamage = damageLeft;
            }
        }
        if (tempStack.Count > 0) charges.Push(Constants.HALF_CHARGE);
        chargesChangedEvent?.Invoke(GetChargesCopy());
    }

    public int GetAvailableCharges() => charges.Count(charge => !IsHalf(charge));

    public int GetMaxCharges() => maxCharges;

    public int GetCurrentCharges() => charges.Count;

    public float GetLastCharge() => charges.Peek();

    public int GetLastChargeIndex() => charges.Count - 1;

    public List<float> GetChargesCopy() => new List<float>(new Stack<float>(charges));

    bool IsHalf(float value) => Mathf.Abs(value - Constants.HALF_CHARGE) < 0.0000000000000001f;

}
