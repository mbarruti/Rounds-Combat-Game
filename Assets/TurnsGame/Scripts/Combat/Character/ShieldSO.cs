using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSO", menuName = "Scriptable Objects/Shield")]
public class ShieldSO : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; } = "None";
    [field: SerializeField] public CharacterActionSO SpecialAction { get; private set; }

    [field: Header("Data")]
    [field: SerializeField] public int MaxCharges { get; private set; } = 3;
    [field: SerializeField] public float ParryChance { get; private set; } = 0.05f;
    //[field: SerializeField] public float ChargeRegen { get; private set; } = 0.5f;
}
