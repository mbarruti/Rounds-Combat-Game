using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ShieldMeterUI : MonoBehaviour
{
    ShieldMeter playerMeter;

    [SerializeField] ChargeUI chargeBar;
    List<ChargeUI> chargeBarList = new();
    Stack<GameObject> chargeBars = new();
    List<float> chargesCopy = new();

    Vector2 barPosition;
    float offset = 60;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Setup(ShieldMeter meter)
    {
        playerMeter = meter;

        if (playerMeter.GetMaxCharges() > 0)
        {
            for (int i = 0; i < playerMeter.GetAvailableCharges(); i++)
            {
                //barPosition.x += offset;
                var bar = Instantiate(chargeBar, transform);
                bar.GetComponent<Image>().color = Color.blue;
                //var barUI = bar.GetComponent<ChargeUI>();
                chargeBarList.Add(bar);
                //chargeBars.Push(bar);
            }
        }
        playerMeter.chargesChangedEvent += UpdateMeterUI;
    }

    //public IEnumerator UpdateChargeBars()
    //{
    //    if (playerMeter.GetAvailableCharges() == chargeBarList.Count())
    //        yield return chargeBarList[playerMeter.GetLastChargeIndex()].GetComponent<Image>().color = Color.green;
    //    else if (playerMeter.GetAvailableCharges() < chargeBarList.Count())
    //        chargeBarList[playerMeter.GetLastChargeIndex()].SetActive(false);
    //    else
    //    {
    //        chargeBarList[playerMeter.GetLastChargeIndex()].GetComponent<Image>().color = Color.red;
    //        chargeBarList[playerMeter.GetLastChargeIndex()].SetActive(true);
    //    }
    //    yield return null;
    //}

    public void SetChargesCopy()
    {
        chargesCopy = playerMeter.GetChargesCopy();
    }

    public IEnumerator RecoverChargeBars()
    {
        yield break;
    }

    public IEnumerator LoseChargeBars(float meterDamage, float waitTime = 0)
    {
        //Stack<GameObject> tempStack = new Stack<GameObject>();

        //while (GetAvailableCharges() > 0 && meterDamage > 0)
        //{
        //    float chargeValue = chargesCopy.Pop();
        //    GameObject chargeBar = chargeBars.Pop();
        //    if (chargeBar.GetComponent<Image>().color == Color.red) // If it's 0.5f
        //    {
        //        tempStack.Push(chargeBar);
        //        barPosition.x -= offset;
        //    }
        //    else // If it's 1f
        //    {
        //        float damageLeft = meterDamage - chargeValue;
        //        chargeValue -= meterDamage;
        //        Debug.Log("entra");
        //        Destroy(chargeBar);
        //        barPosition.x -= offset;
        //        meterDamage = damageLeft;
        //    }
        //}
        //if (tempStack.Count() > 0)
        //{
        //    chargeBar = tempStack.Pop();
        //    chargeBar.transform.position = barPosition;
        //}
        yield return new WaitForSeconds(waitTime);
    }

    void UpdateMeterUI(List<float> currentCharges)
    {
        int i = 1;
        while (chargesCopy.Count >= currentCharges.Count)
        {
            if (TryGet(currentCharges, i, out float charge))
            {
                if (charge != chargesCopy[^i])
                {
                    chargesCopy[^i] = charge;
                    chargeBarList[^i].UpdateBarColor(charge);
                    //return;
                }
                else if (charge == 1f)
                {
                    Debug.Log("entra");
                    chargesCopy.RemoveAt(chargesCopy.Count - i);
                    Destroy(chargeBarList[chargeBarList.Count - i]);
                    chargeBarList.RemoveAt(chargeBarList.Count - i);
                }
            }
            else
            {
                for (int index = 1; index < chargesCopy.Count; index++)
                {
                    if (chargesCopy[^index] == 1f)
                    {
                        chargesCopy.RemoveAt(chargesCopy.Count - index);
                        Destroy(chargeBarList[chargeBarList.Count - index]);
                        chargeBarList.RemoveAt(chargeBarList.Count - index);
                        break;
                    }
                }
            }
        }

    }

    public int GetAvailableCharges() => chargeBars.Count(charge => chargeBars.Peek().GetComponent<Image>().color != Color.red);

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

    bool IsHalf(float value) => Mathf.Abs(value - 0.5f) < 0.0001f;
}