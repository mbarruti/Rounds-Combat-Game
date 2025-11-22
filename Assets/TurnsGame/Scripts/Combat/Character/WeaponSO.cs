using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Sword";

    [field: Header("One-handed")]
    [field: SerializeField] public float BaseDamage { get; private set; } = 20;
    [field: SerializeField] public float MeterDamage { get; private set; } = 1;
    [field: SerializeField] public float Accuracy { get; private set; } = 1;
    [field: SerializeField] public float Prowess { get; private set; } = 1;
    [field: SerializeField] public float CounterChance { get; private set; } = 0.05f;
    [field: SerializeField] public int NumHits { get; private set; } = 1;

    [field: Header("Dual")]
    [field: SerializeField] public float DualBaseDamage { get; private set; } = 20;
    [field: SerializeField] public float DualMeterDamage { get; private set; } = 1;
    [field: SerializeField] public float DualAccuracy { get; private set; } = 1;
    [field: SerializeField] public float DualProwess { get; private set; } = 1;
    [field: SerializeField] public float DualCounterChance { get; private set; } = 0.05f;
    [field: SerializeField] public int DualNumHits { get; private set; } = 1;
}
