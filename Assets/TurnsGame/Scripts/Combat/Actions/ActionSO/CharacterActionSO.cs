using MyProject;
using UnityEngine;
using static MyProject.Constants;

public abstract class CharacterActionSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "None";
    [field: SerializeField] public ActionPriority Lead { get; private set; } = NONE;
    [field: SerializeField] public float MeterCost { get; private set; } = 0;
    [field: SerializeField] public bool CanRecoverMeter { get; private set; } = false;

    public abstract CharacterAction CreateAction();
}
