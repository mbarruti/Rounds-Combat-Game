using UnityEngine;

public abstract class Stance
{
    protected CharacterManager Player { get; private set; }
    protected bool Applied { get; set; }

    public Stance(CharacterManager player)
    {
        Player = player;
        Applied = false;
    }

    public abstract void EnterStance();
    public abstract Stance Clone();
    protected abstract void ExitStance();
    protected abstract void PrepareStanceExit();
}
