using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    public TextMeshProUGUI panelText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;

    public ShieldMeterUI playerShieldMeter;
    public ShieldMeterUI enemyShieldMeter;

    public RectTransform uiPlayerOnePosition;
    public RectTransform uiPlayerTwoPosition;

    private static Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public IEnumerator ExecuteAnimations()
    {
        //Debug.Log("Executing animations");
        int count = coroutineQueue.Count();
        for (int i = 0; i < count; i++)
        {   
            IEnumerator enumerator = coroutineQueue.Dequeue();
            //Debug.LogFormat("Dequed! {0}", i+1);
            yield return StartCoroutine(enumerator);
        }
        if (CombatManager.GetInstance().state != CombatState.END) CombatManager.GetInstance().state = CombatState.CHOOSE;
    }

    public static void AddAnimation(IEnumerator animation)
    {
        //Debug.LogFormat("Enqueuing animation");
        coroutineQueue.Enqueue(animation);
        //Debug.LogFormat("Num of queued animations {0}", coroutineQueue.Count());
    }

    public IEnumerator WriteText(string message, float delay = 0.01f, float waitTime = 0f)
    {
        panelText.text = "";
        foreach (char c in message)
        {
            panelText.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator UpdateHPText(CharacterManager character, float waitTime = 0f)
    {
        yield return character.healthText.text = $"{character.currentHP}/{character.maxHP}";
        yield return new WaitForSeconds(waitTime);
    }
}