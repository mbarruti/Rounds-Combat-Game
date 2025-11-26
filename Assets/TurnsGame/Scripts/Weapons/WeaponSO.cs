using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; } = "None";
    [field: SerializeField] public CharacterActionSO SpecialAction { get; private set; }
    [field: SerializeField] public List<CharacterActionSO> SpecialActions { get; protected set; }

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

public static class ScriptableObjectUtils
{
    public static object GetAsset(string SOName, Type type)
    {
        string[] matchingAssets = AssetDatabase.FindAssets(
            SOName, new string[]{"Assets/TurnsGame/Prefabs/Character Actions"});

        foreach (var asset in matchingAssets)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(asset);
            var SO = AssetDatabase.LoadAssetAtPath(SOpath, type);
            return SO;
        }

        return null;
    }
}
