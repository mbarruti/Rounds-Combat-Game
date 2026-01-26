using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class ParallelAnimation : AnimationStep
{
    private readonly List<AnimationStep> steps;

    public ParallelAnimation(params AnimationStep[] steps)
    {
        this.steps = new List<AnimationStep>(steps);
    }

    public override async UniTask ExecuteAsync(MonoBehaviour runner)
    {
        var tasks = new List<UniTask>(steps.Count);

        foreach (var step in steps)
        {
            tasks.Add(step.ExecuteAsync(runner));
        }

        await UniTask.WhenAll(tasks);
    }
}
