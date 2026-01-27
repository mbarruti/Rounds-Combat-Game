using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static MyProject.Constants;

public class ShieldMeterUI : MonoBehaviour
{
    ShieldMeter playerMeter;

    [SerializeField] ChargeUI chargeBar;
    List<ChargeUI> chargeBarList = new();
    List<float> previousCharges = new();

    public void Setup(ShieldMeter meter)
    {
        playerMeter = meter;
        SetChargesCopy();

        if (playerMeter.GetMaxCharges() > 0)
        {
            for (int i = 0; i < previousCharges.Count; i++)
            {
                var bar = Instantiate(chargeBar, transform);
                bar.UpdateBarColor(previousCharges[i]);
                chargeBarList.Add(bar);
            }
        }
        playerMeter.chargesWillChangeEvent += SetChargesCopy;
        playerMeter.chargesChangedEvent += UpdateMeterUI;
    }

    public void SetChargesCopy()
    {
        previousCharges = playerMeter.GetChargesCopy();
    }

    public async UniTask RecoverChargeBars(List<float> currentCharges, List<float> copy,
    float waitTime = 0f)
    {
        for (int i = 0; i < currentCharges.Count; i++)
        {
            if (TryGet(copy, i, out float charge))
            {
                if (charge != currentCharges[i])
                {
                    copy[i] = currentCharges[i];
                    chargeBarList[i].UpdateBarColor(copy[i]);
                }
            }
            else
            {
                copy.Add(currentCharges[i]);
                var bar = Instantiate(chargeBar, transform);
                bar.UpdateBarColor(copy[i]);
                chargeBarList.Add(bar);
            }
        }
            await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask LoseChargeBars(List<float> currentCharges, List<float> copy,
    float waitTime = 0f)
    {
        int lastIndex = copy.Count - 1;
        int aux = 0;
        while (copy.Count > currentCharges.Count)
        {
            for (int i = 1; i <= copy.Count; i++)
            {
                float value = copy[^i];
                if (value == FULL_CHARGE)
                {
                    copy.RemoveAt(copy.Count - i);
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
        if (currentCharges.Count != 0 && copy[^1] != currentCharges[^1])
        {
            copy[^1] = HALF_CHARGE;
            chargeBarList[^1].UpdateBarColor(copy[^1]);
        }
        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    void UpdateMeterUI(List<float> currentCharges)
    {
        List<float> previousChargesCopy = new(previousCharges);

        if (previousChargesCopy.Count > currentCharges.Count)
        {
            Anim.Sequence(Anim.Do(() => LoseChargeBars(currentCharges, previousChargesCopy)));
        }
        else if (previousChargesCopy.Count < currentCharges.Count)
            Anim.Sequence(
                Anim.Do(() =>
                    RecoverChargeBars(currentCharges, previousChargesCopy)
                )
            );
        else if (previousChargesCopy[^1] != currentCharges[^1] && currentCharges[^1] != FULL_CHARGE)
            Anim.Sequence(
                Anim.Do(() =>
                    LoseChargeBars(currentCharges, previousChargesCopy)
                )
            );
        else if (previousChargesCopy[^1] != currentCharges[^1] && currentCharges[^1] == FULL_CHARGE)
            Anim.Sequence(
                Anim.Do(() =>
                    RecoverChargeBars(currentCharges, previousChargesCopy)
                )
            );
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
