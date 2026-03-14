using UnityEngine;

public class AnimatorTask
{
    Animator animator;
    int layer;

    public AnimatorTask(Animator animator, int layer = 0)
    {
        this.animator = animator;
        this.layer = layer;
    }

    /// <summary>
    /// Plays an animation and returns its controller
    /// </summary>
    public AnimationAwaiter Play(string state, float fade = 0.2f)
    {
        animator.CrossFadeInFixedTime(state, fade);
        return new AnimationAwaiter(animator, state, layer);
    }
}
