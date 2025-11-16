using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance { get; private set; }

    public TextMeshProUGUI panelText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;

    public ShieldMeterUI playerShieldMeter;
    public ShieldMeterUI enemyShieldMeter;

    public RectTransform uiPlayerOnePosition;
    public RectTransform uiPlayerTwoPosition;

    private static readonly Queue<IEnumerator> coroutineQueue = new();

    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject blockButton;
    [SerializeField] GameObject chargeButton;
    [SerializeField] GameObject tackleButton;

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
        int count = coroutineQueue.Count;
        for (int i = 0; i < count; i++)
        {
            IEnumerator enumerator = coroutineQueue.Dequeue();
            //Debug.LogFormat("Dequed! {0}", i+1);
            yield return StartCoroutine(enumerator);
        }
        if (CombatManager.Instance.state != CombatState.END) CombatManager.Instance.RoundStart();
    }

    public static void AddAnimation(IEnumerator animation)
    {
        //Debug.LogFormat("Enqueuing animation");
        coroutineQueue.Enqueue(animation);
        //Debug.LogFormat("Num of queued animations {0}", coroutineQueue.Count());
    }

    public IEnumerator WriteText(string message, float delay = 0.03f, float waitTime = 1f)
    {
        //TO-DO: make dialogue animation and HP update when taking damage happen at the same time
        panelText.text = "";
        foreach (char c in message)
        {
            panelText.text += c;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator UpdateHPText(CharacterManager character, float currentHP,
    float waitTime = 0.5f)
    {
        yield return character.healthText.text = $"{Mathf.CeilToInt(currentHP)}/{(int)character.maxHP}";
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator ShowActionButtons(CharacterActionController actionController, float waitTime = 0f)
    {
        if (actionController.canUseAttack) attackButton.SetActive(true);
        if (actionController.canUseBlock) blockButton.SetActive(true);
        if (actionController.canUseCharge) chargeButton.SetActive(true);
        if (actionController.canUseTackle) tackleButton.SetActive(true);
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator HideActionButtons(float waitTime = 0f)
    {
        attackButton.SetActive(false);
        blockButton.SetActive(false);
        chargeButton.SetActive(false);
        tackleButton.SetActive(false);
        yield return new WaitForSeconds(waitTime);
    }
}
