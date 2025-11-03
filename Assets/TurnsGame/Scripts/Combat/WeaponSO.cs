using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public float BaseDamage { get; private set; } = 20;
    [field: SerializeField] public float MeterDamage { get; private set; } = 1;
    [field: SerializeField] public float Accuracy { get; private set; } = 1;
    [field: SerializeField] public float Prowess { get; private set; } = 1;
}
