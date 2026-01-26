using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    private static readonly List<List<AnimationStep>> animationList = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public static void Sequence(params AnimationStep[] steps)
    {
        animationList.Add(new List<AnimationStep>(steps));
    }

    // public static void Sequence(params AnimationStep[] steps)
    // {
    //     Instance.StartCoroutine(RunSequence(steps));
    // }


    private async UniTask RunSequenceAsync(List<AnimationStep> sequence)
    {
        foreach (var step in sequence)
        {
            await step.ExecuteAsync(this);
        }
    }

    public async UniTask RunAnimations()
    {
        foreach (var sequence in animationList)
        {
            await RunSequenceAsync(sequence);
        }

        animationList.Clear();

        if (CombatManager.Instance.state != CombatState.END)
            CombatManager.Instance.RoundStart();
    }

    // private static IEnumerator ForceYield(IEnumerator coroutine)
    // {
    //     yield return null;
    //     yield return coroutine;
    // }

    // Helpers

    public static AnimationStep Do(System.Func<UniTask> task)
        => new AnimationTask(task);

    public static AnimationStep Parallel(params AnimationStep[] steps)
        => new ParallelAnimation(steps);
}

// Helper Builder
public static class Anim
{
    public static AnimationStep Do(System.Func<UniTask> task) => AnimationManager.Do(task);
    public static AnimationStep Parallel(params AnimationStep[] steps) => AnimationManager.Parallel(steps);
    public static void Sequence(params AnimationStep[] steps) => AnimationManager.Sequence(steps);
}

