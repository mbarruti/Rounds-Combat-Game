using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public TextMeshProUGUI panelText;

    private float waitTime = 1f;

    public RectTransform uiPlayerOnePosition;
    public RectTransform uiPlayerTwoPosition;

    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

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
        Debug.Log("Executing animations");
        int count = coroutineQueue.Count();
        for (int i = 0; i < count; i++)
        {   
            IEnumerator enumerator = coroutineQueue.Dequeue();
            Debug.LogFormat("Dequed! {0}", i+1);
            yield return StartCoroutine(enumerator);
            if (i < count-1) yield return new WaitForSeconds(waitTime);
        }
        CombatManager.GetInstance().state = CombatState.CHOOSE;
    }

    public void WriteText(string message, float delay = 0.03f)
    {
        Debug.LogFormat("Enqueuing text coroutine: {0}", message);
        coroutineQueue.Enqueue(TypeTextCoroutine(message, delay));
        Debug.LogFormat("Num of queued texts {0}", coroutineQueue.Count());
    }

    private IEnumerator TypeTextCoroutine(string message, float delay)
    {
        panelText.text = "";
        foreach (char c in message)
        {
            panelText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}