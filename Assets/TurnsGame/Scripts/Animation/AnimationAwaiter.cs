using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AnimationAwaiter
{
    Animator animator;
    string state;
    int layer;

    public AnimationAwaiter(Animator animator, string state, int layer)
    {
        this.animator = animator;
        this.state = state;
        this.layer = layer;
    }

    /// <summary>
    /// Waits for the animator to enter the animation's state
    /// </summary>
    async UniTask WaitEnter()
    {
        await UniTask.WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(layer).IsName(state));
    }

    /// <summary>
    /// Waits until the animation reaches a given time
    /// </summary>
    public async UniTask At(float normalizedTime)
    {
        if (!animator.GetCurrentAnimatorStateInfo(layer).IsName(state)) await WaitEnter();

        await UniTask.WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= normalizedTime);
    }

    /// <summary>
    /// Waits until the animation finishes
    /// </summary>
    public async UniTask End()
    {
        if (!animator.GetCurrentAnimatorStateInfo(layer).IsName(state)) await WaitEnter();

        await UniTask.WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 1f);
    }

    /// <summary>
    /// Waits for given seconds to continue
    /// </summary>
    public async UniTask Seconds(float seconds)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds));
    }
}
