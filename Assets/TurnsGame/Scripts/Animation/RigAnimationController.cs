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

    public RigAnimationController(CharacterManager player, Animator playerAnimator)
    {
        Player = player;
        PlayerAnimator = playerAnimator;
        Anim = new(playerAnimator);
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

    public async UniTask ActionAnimation(string anim, float trigger = 1f)
    {
        // TODO: FIND A WAY TO WAIT FOR AN ANIMATION TO FINISH (IF REQUIRED) BEFORE PLAYING NEXT ONE
        var actionAnim = Anim.Play(anim);

        if (trigger < 1f)
        {
            Debug.Log($"{Player.username} enters in {anim}");
            _ = UniTask.Create(async () =>
            {
                await actionAnim.End();

                var state = Player.animator.GetCurrentAnimatorStateInfo(0);

                if (!PlayerAnimator.IsInTransition(0) && state.IsName(anim))
                    PlayerAnimator.CrossFadeInFixedTime("DefaultIdle", 0.2f);

            });
            await actionAnim.At(trigger);
        }
        else
        {
            await actionAnim.End();

            var state = Player.animator.GetCurrentAnimatorStateInfo(0);

            if (!PlayerAnimator.IsInTransition(0) && state.IsName(anim))
                PlayerAnimator.CrossFadeInFixedTime("DefaultIdle", 0.2f);
        }
    }
}
