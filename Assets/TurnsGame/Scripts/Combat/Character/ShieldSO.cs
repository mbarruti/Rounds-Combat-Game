using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSO", menuName = "Scriptable Objects/Shield")]
public class ShieldSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Shield";
    [field: SerializeField] public int MaxCharges { get; private set; } = 3;
    [field: SerializeField] public float ParryChance { get; private set; } = 0.05f;
    //[field: SerializeField] public float ChargeRegen { get; private set; } = 0.5f;

}
