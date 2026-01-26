using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class AnimationStep
{
    public abstract UniTask ExecuteAsync(MonoBehaviour runner);
}
