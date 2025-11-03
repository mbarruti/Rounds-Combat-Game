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
        int lastIndex = chargesCopy.Count - 1;
        int aux = 0;
        //Debug.Log("currentcharges count: " + currentCharges.Count);
        while (chargesCopy.Count > currentCharges.Count)
        {
            if (!TryGet(currentCharges, lastIndex, out float charge))
            {
                for (int i = 1; i <= chargesCopy.Count; i++)
                {
                    float value = chargesCopy[^i];
                    if (value == 1f)
                    {
                        //Debug.Log("if condition satisfied");
                        chargesCopy.RemoveAt(chargesCopy.Count - i);
                        Destroy(chargeBarList[chargeBarList.Count - i].gameObject);
                        chargeBarList.RemoveAt(chargeBarList.Count - i);
                        lastIndex--;
                        break;
                    }
                }
            }
            else
            {
                chargesCopy.RemoveAt(chargesCopy.Count - 1);
                Destroy(chargeBarList[chargeBarList.Count - 1].gameObject);
                chargeBarList.RemoveAt(chargeBarList.Count - 1);
                lastIndex--;
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