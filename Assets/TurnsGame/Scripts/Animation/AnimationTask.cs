using Cysharp.Threading.Tasks;
using System;

public class AnimationTask : AnimationStep
{
    private readonly Func<UniTask> task;

    public AnimationTask(Func<UniTask> task)
    {
        this.task = task;
    }

    public override UniTask ExecuteAsync(UnityEngine.MonoBehaviour runner)
    {
        return task();
    }
}
