using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MyProject.Constants;

[System.Serializable]
public class ShieldMeter
{
    [SerializeField] int maxCharges;
    Stack<float> charges;
    public event Action chargesWillChangeEvent;
    public event Action<List<float>> chargesChangedEvent;

    public void Setup(int shieldMaxCharges)
    {
        charges = new Stack<float>();
        maxCharges = shieldMaxCharges;
        Refill();
    }

    public void Refill()
    {
        charges.Clear();
        for (int i = 0; i < maxCharges; i++)
        {
            charges.Push(FULL_CHARGE);
        }
    }

    public void RecoverCharges()
    {
        chargesWillChangeEvent?.Invoke();
        if (GetAvailableCharges() == maxCharges)
            return;
        if (charges.Count > 0 && IsHalf(charges.Peek()))
        {
            float charge = charges.Pop();
            charge = FULL_CHARGE;
            charges.Push(charge);
        }
        else
            charges.Push(HALF_CHARGE);
        chargesChangedEvent?.Invoke(GetChargesCopy());
    }

    public void LoseCharges(float meterLoss)
    {
        chargesWillChangeEvent?.Invoke();

        Stack<float> tempStack = new Stack<float>();

        while (GetAvailableCharges() > 0 && meterLoss > 0)
        {
            float chargeValue = charges.Pop();
            if (IsHalf(chargeValue)) // If charge is recovering
            {
                tempStack.Push(chargeValue);
            }
            else // If charge is available
            {
                float damageLeft = meterLoss - chargeValue;
                chargeValue -= meterLoss;
                if (IsHalf(chargeValue))
                    tempStack.Push(chargeValue);
                meterLoss = damageLeft;
            }
        }
        if (tempStack.Count > 0) charges.Push(HALF_CHARGE);
        chargesChangedEvent?.Invoke(GetChargesCopy());
    }

    public int GetAvailableCharges() => charges.Count(charge => !IsHalf(charge));

    public int GetMaxCharges() => maxCharges;

    public int GetCurrentCharges() => charges.Count;

    public float GetLastCharge() => charges.Peek();

    public int GetLastChargeIndex() => charges.Count - 1;

    public List<float> GetChargesCopy() => new List<float>(new Stack<float>(charges));

    bool IsHalf(float value) => Mathf.Abs(value - HALF_CHARGE) < 0.0000000000000001f;

}
