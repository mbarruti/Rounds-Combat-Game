using MyProject;
using UnityEngine;
using static MyProject.Constants;

public abstract class CharacterActionSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "None";
    [field: SerializeField] public ActionPriority Lead { get; private set; } = NONE;
    [field: SerializeField] public bool CanRecoverMeter { get; private set; }

    public abstract CharacterAction CreateAction();
}
