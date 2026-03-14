using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RigAnimationController
{
    CharacterManager Player { get; set; }
    Animator PlayerAnimator { get; set; }
    AnimatorTask Anim { get; set; }
    List<string> rigAnimationList = new();
    UniTaskCompletionSource animationCompletionSource;
    UniTaskCompletionSource frameCompletionSource;

    public event Action OnFrame;

    public RigAnimationController(CharacterManager player, Animator playerAnimator)
    {
        Player = player;
        PlayerAnimator = playerAnimator;
        Anim = new(playerAnimator);
    }

    // public void ReturnToIdle()
    // {
    //     IdleAnimation()
    // }

    public void DeleteFrameEvents()
    {
        OnFrame = null;
    }

    public void OnAnimationFrame()
    {
        //OnFrame?.Invoke();
        frameCompletionSource?.TrySetResult();
        frameCompletionSource = null;
    }

    public void OnAnimationFinished()
    {
        animationCompletionSource?.TrySetResult();
        animationCompletionSource = null;
    }

    public async UniTask Move(float targetZ, float waitTime = 0f)
    {
        if (Player.transform.position.z == targetZ) return;

        float speed = 7.5f;

        Vector3 targetPosition = new Vector3(
            Player.transform.position.x,
            Player.transform.position.y,
            targetZ
        );

        Player.transform.LookAt(targetPosition);
        PlayerAnimator.CrossFadeInFixedTime("Run", 0.2f);

        while (Mathf.Abs(Player.transform.position.z - targetZ) > 0.01f)
        {
            Player.transform.position = Vector3.MoveTowards(
                Player.transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        Player.transform.position = targetPosition;
        PlayerAnimator.CrossFadeInFixedTime("DefaultIdle", 0.2f);

        // if (rigAnimationList.Count == 1)
        //     PlayerAnimator.Play("DefaultIdle");

        // if (rigAnimationList.Count > 0)
        //     rigAnimationList.RemoveAt(0);

        if (waitTime > 0f)
        {
            await UniTask.Delay(
                Mathf.RoundToInt(waitTime * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update
            );
        }
    }

    public float CalculateTargetZ(float enemyZ)
    {
        float desiredDistance = 1.5f;

        bool iAmBehind = Player.expectedPosition.z < enemyZ;

        float targetZ = iAmBehind
            ? enemyZ - desiredDistance
            : enemyZ + desiredDistance;

        return targetZ;
    }

    public async UniTask IdleAnimation(string idleAnim, float waitTime = 0)
    {
        Player.animator.CrossFadeInFixedTime(idleAnim, 0.2f);

        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask ActionAnimation(string anim, float trigger)
    {
/*         Player.animator.CrossFadeInFixedTime(anim, 0.2f);

        // Esperar a que el animator entre en el estado
        await UniTask.WaitUntil(() =>
            Player.animator.GetCurrentAnimatorStateInfo(0).IsName(anim)
        );

        _ = UniTask.Create(async () =>
        {
            await UniTask.WaitUntil(() =>
                Player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            );

            if (Player.animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
                Player.animator.CrossFadeInFixedTime("DefaultIdle", 0.2f);
        });

        // Esperar a que termine la animación
        await UniTask.WaitUntil(() =>
            Player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= animEndTime
        ); */
        var actionAnim = Anim.Play(anim);
        _ = UniTask.Create(async () =>
        {
            await actionAnim.End();

            var state = Player.animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(anim))
                Player.animator.CrossFadeInFixedTime("DefaultIdle", 0.2f);
        });
        if (trigger != 1f) await actionAnim.At(trigger);
    }
}
