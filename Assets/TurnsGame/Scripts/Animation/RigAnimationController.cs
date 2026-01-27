using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RigAnimationController
{
    CharacterManager Player { get; set; }
    Animator PlayerAnimator { get; set; }
    List<Func<UniTask>> rigAnimationList = new();

    public RigAnimationController(CharacterManager player, Animator playerAnimator)
    {
        Player = player;
        PlayerAnimator = playerAnimator;
    }

    // public void ReturnToIdle()
    // {
    //     IdleAnimation()
    // }

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
        Player.isPerformingAction = true;
        Player.animator.CrossFadeInFixedTime(idleAnim, 0.2f);

        await UniTask.Delay(
            Mathf.RoundToInt(waitTime * 1000),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update
        );
    }

    public async UniTask ActionAnimation(string anim)
    {
        Player.isPerformingAction = true;

        Player.animator.CrossFadeInFixedTime(anim, 0.2f);

        while (Player.isPerformingAction)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public Func<UniTask> RigAnimation(Func<UniTask> task)
    {
        rigAnimationList.Add(task);
        // AnimationManager.Sequence(
        //     AnimationManager.Do(anim)
        // );
        return task;
    }
}
