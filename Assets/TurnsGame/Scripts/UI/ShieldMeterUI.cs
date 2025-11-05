using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMeterUI : MonoBehaviour
{
    ShieldMeter playerMeter;

    [SerializeField] ChargeUI chargeBar;
    List<ChargeUI> chargeBarList = new();
    List<float> chargesCopy = new();

    public void Setup(ShieldMeter meter)
    {
        playerMeter = meter;
        SetChargesCopy();

        if (playerMeter.GetMaxCharges() > 0)
        {
            for (int i = 0; i < chargesCopy.Count; i++)
            {
                var bar = Instantiate(chargeBar, transform);
                bar.UpdateBarColor(chargesCopy[i]);
                chargeBarList.Add(bar);
            }
        }
        playerMeter.chargesChangedEvent += UpdateMeterUI;
    }

    public void SetChargesCopy()
    {
        chargesCopy = playerMeter.GetChargesCopy();
    }

    public IEnumerator RecoverChargeBars(List<float> currentCharges, float waitTime = 0f)
    {
        for (int i = 0; i < currentCharges.Count; i++)
        {
            if (TryGet(chargesCopy, i, out float charge))
            {
                if (charge != currentCharges[i])
                {
                    chargesCopy[i] = currentCharges[i];
                    chargeBarList[i].UpdateBarColor(chargesCopy[i]);
                }
            }
            else
            {
                chargesCopy.Add(currentCharges[i]);
                var bar = Instantiate(chargeBar, transform);
                bar.UpdateBarColor(chargesCopy[i]);
                chargeBarList.Add(bar);
            }
        }
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator LoseChargeBars(List<float> currentCharges, float waitTime = 0f)
    {
        int lastIndex = chargesCopy.Count - 1;
        int aux = 0;
        while (chargesCopy.Count > currentCharges.Count)
        {
            for (int i = 1; i <= chargesCopy.Count; i++)
            {
                float value = chargesCopy[^i];
                if (value == 1f)
                {
                    chargesCopy.RemoveAt(chargesCopy.Count - i);
                    Destroy(chargeBarList[chargeBarList.Count - i].gameObject);
                    chargeBarList.RemoveAt(chargeBarList.Count - i);
                    lastIndex--;
                    break;
                }
            }
            if (aux == 15)
            {
                Debug.Log("aux reached max");
                break;
            }
            else aux++;
        }
        if (currentCharges.Count != 0 && chargesCopy[^1] != currentCharges[^1])
        {
            chargesCopy[^1] = 0.5f;
            chargeBarList[^1].UpdateBarColor(chargesCopy[^1]);
        }
        yield return new WaitForSeconds(waitTime);
    }

    void UpdateMeterUI(List<float> currentCharges)
    {
        if (chargesCopy.Count > currentCharges.Count)
            UI.AddAnimation(LoseChargeBars(currentCharges));
        else if (chargesCopy.Count < currentCharges.Count)
            UI.AddAnimation(RecoverChargeBars(currentCharges));
        else if (chargesCopy[^1] != currentCharges[^1] && currentCharges[^1] != 1f)
            UI.AddAnimation(LoseChargeBars(currentCharges));
        else if (chargesCopy[^1] != currentCharges[^1] && currentCharges[^1] == 1f)
            UI.AddAnimation(RecoverChargeBars(currentCharges));
    }

    public bool TryGet(List<float> list, Index index, out float value)
    {
        int i = index.IsFromEnd ? list.Count - index.Value : index.Value;

        if (i >= 0 && i < list.Count)
        {
            value = list[i];
            return true;
        }

        value = default!;
        return false;
    }
}