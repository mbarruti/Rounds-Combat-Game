using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

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

    public static readonly Queue<IEnumerator> coroutineQueue = new();

    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject blockButton;
    [SerializeField] GameObject nothingButton;
    [SerializeField] public GameObject weaponSpecialButton;
    [SerializeField] public GameObject weaponSpecialButton2;
    [SerializeField] public GameObject shieldSpecialButton;

    [SerializeField] GameObject chargeButton;
    [SerializeField] GameObject tackleButton;
    [SerializeField] GameObject parryButton;

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

    public async UniTask WriteText(string message, float delay = 0.03f, float waitTime = 1f)
    {
        panelText.text = "";

        foreach (char c in message)
        {
            panelText.text += c;

            await UniTask.Delay(
                Mathf.RoundToInt(delay * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update
            );
        }

        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask UpdateHPText(CharacterManager character, float currentHP,
    float waitTime = 0.5f)
    {
        character.healthText.text = $"{Mathf.CeilToInt(currentHP)}/{(int)character.maxHP}";
        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask ShowActionButtons(CharacterActionController actionController,
    float waitTime = 0f)
    {
        attackButton.SetActive(true);
        blockButton.SetActive(true);
        weaponSpecialButton.SetActive(true);
        weaponSpecialButton2.SetActive(true);
        shieldSpecialButton.SetActive(true);
        nothingButton.SetActive(true);
        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask HideActionButtons(float waitTime = 0f)
    {
        attackButton.SetActive(false);
        blockButton.SetActive(false);
        weaponSpecialButton.SetActive(false);
        weaponSpecialButton2.SetActive(false);
        shieldSpecialButton.SetActive(false);
        nothingButton.SetActive(false);
        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }
}
